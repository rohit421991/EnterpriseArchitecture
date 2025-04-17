using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Manager;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enterprise.Tests.Employees
{
    public class EmployeesServiceTests
    {
        private readonly Mock<IEmployeesRepository> _mockEmployeesRepository;
        private readonly EmployeesService _employeesService;

        public EmployeesServiceTests()
        {
            _mockEmployeesRepository = new Mock<IEmployeesRepository>();
            _employeesService = new EmployeesService(_mockEmployeesRepository.Object);
        }

        [Fact]
        public async Task GetAllEmployeeAsync_ReturnsListOfEmployeeDtos()
        {
            // Arrange
            var mockEmployees = new List<Enterprise.Data.Entities.Employees>
            {
                new Enterprise.Data.Entities.Employees { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Mobile = "1234567890", Address = "123 Main St" },
                new Enterprise.Data.Entities.Employees { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Mobile = "9876543210", Address = "456 Elm St" }
            };

            _mockEmployeesRepository
                .Setup(repo => repo.GetAllEmployeesAsync())
                .ReturnsAsync(mockEmployees);

            // Act
            var result = await _employeesService.GetAllEmployeeAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<EmployeeDto>>(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John", result[0].FirstName);
            Assert.Equal("Jane", result[1].FirstName);

            // Verify that the repository method was called exactly once
            _mockEmployeesRepository.Verify(repo => repo.GetAllEmployeesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeAsync_WhenEmployeeExists()
        {
            // Arrange
            var mockEmployee = new Enterprise.Data.Entities.Employees
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Mobile = "1234567890",
                Address = "123 Main St"
            };

            _mockEmployeesRepository
                .Setup(repo => repo.GetEmployeeAsync(1))
                .ReturnsAsync(mockEmployee);

            // Act
            var result = await _employeesService.GetEmployeeAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<EmployeeDto>(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);

            // Verify that the repository method was called exactly once
            _mockEmployeesRepository.Verify(repo => repo.GetEmployeeAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeAsync_WhenEmployeeDoesNotExist()
        {
            // Arrange
            _mockEmployeesRepository
                .Setup(repo => repo.GetEmployeeAsync(1))
                .ReturnsAsync((Enterprise.Data.Entities.Employees?)null); // Explicitly mark as nullable

            // Act
            var result = await _employeesService.GetEmployeeAsync(1);

            // Assert
            Assert.Null(result);

            // Verify that the repository method was called exactly once
            _mockEmployeesRepository.Verify(repo => repo.GetEmployeeAsync(1), Times.Once);
        }

        [Fact]
        public async Task SaveEmployeeAsync_Successfully()
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

            _mockEmployeesRepository
                .Setup(repo => repo.SaveEmployeeAsync(It.IsAny<Enterprise.Data.Entities.Employees>()))
                .ReturnsAsync(true);

            // Act
            var result = await _employeesService.SaveEmployeeAsync(employeeDto);

            // Assert
            Assert.True(result);

            // Verify that the repository method was called exactly once
            _mockEmployeesRepository.Verify(repo => repo.SaveEmployeeAsync(It.IsAny<Enterprise.Data.Entities.Employees>()), Times.Once);
        }

        [Fact]
        public async Task SaveEmployeeAsync_WhenEmployeeIsNotSaved()
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

            _mockEmployeesRepository
                .Setup(repo => repo.SaveEmployeeAsync(It.IsAny<Enterprise.Data.Entities.Employees>()))
                .ReturnsAsync(false);

            // Act
            var result = await _employeesService.SaveEmployeeAsync(employeeDto);

            // Assert
            Assert.False(result);

            // Verify that the repository method was called exactly once
            _mockEmployeesRepository.Verify(repo => repo.SaveEmployeeAsync(It.IsAny<Enterprise.Data.Entities.Employees>()), Times.Once);
        }
    }
}
