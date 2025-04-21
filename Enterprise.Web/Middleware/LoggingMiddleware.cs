using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Web.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Log the incoming request
                await LogRequest(context);

                // Capture the response body
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    // Proceed to the next middleware
                    await _next(context);

                    // Log the outgoing response
                    await LogResponse(context, responseBody);

                    // Copy the response back to the original stream
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            catch (Exception ex)
            {
                // Handle and log specific exceptions
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            var requestBody = string.Empty;
            if (context.Request.ContentLength > 0)
            {
                using (var reader = new StreamReader(
                    context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            Log.Information("Incoming Request: {Method} {Path} {Body}",
                context.Request.Method,
                context.Request.Path,
                requestBody);
        }

        private async Task LogResponse(HttpContext context, MemoryStream responseBody)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            Log.Information("Outgoing Response: {StatusCode} {Body}",
                context.Response.StatusCode,
                responseText);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Log the exception details
            Log.Error(ex, "An exception occurred while processing the request.");

            // Check for specific exception types and log additional context
            if (ex is InvalidOperationException)
            {
                Log.Warning("InvalidOperationException: {Message}", ex.Message);
            }
            else if (ex is UnauthorizedAccessException)
            {
                Log.Warning("UnauthorizedAccessException: {Message}", ex.Message);
            }
            else
            {
                Log.Error("Unhandled Exception: {Message}", ex.Message);
            }

            // Set the response status code and content
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An internal server error occurred. Please try again later.",
                Details = ex.Message // Optional: Include exception details for debugging
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}

