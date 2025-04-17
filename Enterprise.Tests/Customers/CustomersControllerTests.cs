using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Data.Queries;
using Enterprise.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enterprise.Tests.Customers
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomersService> _mockCustomersService;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockCustomersService = new Mock<ICustomersService>();
            _controller = new CustomersController(_mockCustomersService.Object);
        }

        [Fact]
        public async Task GetCustomers_ReturnsCustomerDto_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var mockCustomer = new CustomerDto
            {
                Id = customerId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            _mockCustomersService
                .Setup(service => service.GetAsync(customerId))
                .ReturnsAsync(mockCustomer);

            // Act
            var result = await _controller.GetCustomers(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CustomerDto>(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("john.doe@example.com", result.Email);

            // Verify that the service method was called exactly once
            _mockCustomersService.Verify(service => service.GetAsync(customerId), Times.Once);
        }

        [Fact]
        public async Task GetCustomers_ReturnsNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _mockCustomersService
                .Setup(service => service.GetAsync(customerId))
                .ReturnsAsync((CustomerDto)null);

            // Act
            var result = await _controller.GetCustomers(customerId);

            // Assert
            Assert.Null(result);

            // Verify that the service method was called exactly once
            _mockCustomersService.Verify(service => service.GetAsync(customerId), Times.Once);
        }
    }
}

