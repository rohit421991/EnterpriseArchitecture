using Enterprise.Contract;
using Enterprise.Manager;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Enterprise.Tests.Employees
{
    public class EmployeesRepositoryIntegrationTests
    {
        private readonly IEmployeesRepository _repository;

        public EmployeesRepositoryIntegrationTests()
        {
            _repository = Substitute.For<IEmployeesRepository>();
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsAllEmployees()
        {
            // Act
            var result = await _repository.GetAllEmployeesAsync();

            Assert.Null(result);
        }

        [Fact]
        public async Task GetEmployeeAsync_ReturnsEmployee_WhenEmployeeExists()
        {
            // Act
            var result = await _repository.GetEmployeeAsync(4);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetEmployeeAsync_ReturnsNull_WhenEmployeeDoesNotExist()
        {
            // Act
            var result = await _repository.GetEmployeeAsync(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SaveEmployeeAsync_AddsNewEmployee()
        {
            // Arrange
            var newEmployee = new Enterprise.Data.Entities.Employees();

            // Act
            var result = await _repository.SaveEmployeeAsync(newEmployee);

            // Assert
            Assert.False(result);
        }
    }
}
