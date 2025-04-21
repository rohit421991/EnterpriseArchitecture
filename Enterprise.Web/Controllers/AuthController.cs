using Enterprise.Manager;
using Enterprise.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Enterprise.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            // Define a list of valid users
            var validUsers = new List<UserLogin>
            {
                new UserLogin { UserName = "admin", Password = "admin@123" },
                new UserLogin { UserName = "john", Password = "john@123" },
                new UserLogin { UserName = "smith", Password = "smith@123" }
            };

            // Check if the provided credentials match any user in the list
            var isValidUser = validUsers.Any(u => u.UserName == user.UserName && u.Password == user.Password);

            if (isValidUser)
            {
                // Assign a role based on the username (optional)
                var role = user.UserName == "admin" ? "Admin" : "User";

                // Generate a token
                var token = _tokenService.GenerateToken(user.UserName, role);
                return Ok(new { token });
            }
            return Unauthorized();
        }
    }
}
