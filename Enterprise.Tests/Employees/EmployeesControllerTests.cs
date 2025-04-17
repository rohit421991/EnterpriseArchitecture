using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Web.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace Enterprise.Tests
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IEmployeesService> _mockEmployeesService;
        private readonly Mock<ILogger<EmployeesController>> _mockLogger;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _mockEmployeesService = new Mock<IEmployeesService>();
            _mockLogger = new Mock<ILogger<EmployeesController>>();
            _controller = new EmployeesController(_mockEmployeesService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsListOfEmployees()
        {
            // Arrange
            var mockEmployees = new List<EmployeeDto>
            {
                new EmployeeDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Mobile = "1234567890", Address = "123 Main St" },
                new EmployeeDto { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Mobile = "9876543210", Address = "456 Elm St" }
            };

            _mockEmployeesService
                .Setup(service => service.GetAllEmployeeAsync())
                .ReturnsAsync(mockEmployees);

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<EmployeeDto>>(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John", result[0].FirstName);
            Assert.Equal("Jane", result[1].FirstName);

            // Verify that the service method was called exactly once
            _mockEmployeesService.Verify(service => service.GetAllEmployeeAsync(), Times.Once);
        }

        [Fact]
        public async Task GetEmployee_ById_WhenEmployeeExists()
        {
            // Arrange
            var mockEmployee = new EmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Mobile = "1234567890",
                Address = "123 Main St"
            };

            _mockEmployeesService
                .Setup(service => service.GetEmployeeAsync(1))
                .ReturnsAsync(mockEmployee);

            // Act
            var result = await _controller.GetEmployee(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<EmployeeDto>(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);

            // Verify that the service method was called exactly once
            _mockEmployeesService.Verify(service => service.GetEmployeeAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetEmployee_WhenEmployeeDoesNotExist()
        {
            // Arrange
            _mockEmployeesService
                .Setup(service => service.GetEmployeeAsync(1))
                .ReturnsAsync((EmployeeDto)null);

            // Act
            var result = await _controller.GetEmployee(1);

            // Assert
            Assert.Null(result);

            // Verify that the service method was called exactly once
            _mockEmployeesService.Verify(service => service.GetEmployeeAsync(1), Times.Once);
        }

        [Fact]
        public async Task SaveEmployee_Successfully()
        {
            // Arrange
            var employeeDto = new EmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Mobile = "1234567890",
                Address = "123 Main St"
            };

            _mockEmployeesService
                .Setup(service => service.SaveEmployeeAsync(employeeDto))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.SaveEmployee(employeeDto);

            // Assert
            Assert.True(result);

            // Verify that the service method was called exactly once
            _mockEmployeesService.Verify(service => service.SaveEmployeeAsync(employeeDto), Times.Once);
        }

        [Fact]
        public async Task SaveEmployee_WhenEmployeeIsNotSaved()
        {
            // Arrange
            var employeeDto = new EmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Mobile = "1234567890",
                Address = "123 Main St"
            };

            _mockEmployeesService
                .Setup(service => service.SaveEmployeeAsync(employeeDto))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.SaveEmployee(employeeDto);

            // Assert
            Assert.False(result);

            // Verify that the service method was called exactly once
            _mockEmployeesService.Verify(service => service.SaveEmployeeAsync(employeeDto), Times.Once);
        }
    }
}
