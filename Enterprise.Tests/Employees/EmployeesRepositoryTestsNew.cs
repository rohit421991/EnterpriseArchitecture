using Enterprise.Data.Entities;
using Enterprise.Manager;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Enterprise.Tests.Employees
{
    public class EmployeesRepositoryMockTests
    {
        private readonly Mock<FbConnection> _mockConnection;
        private readonly Mock<FbCommand> _mockCommand;
        private readonly Mock<FbDataReader> _mockReader;
        private readonly EmployeesRepository _repository;

        public EmployeesRepositoryMockTests()
        {
            // Mock the Firebird connection, command, and reader
            _mockConnection = new Mock<FbConnection>();
            _mockCommand = new Mock<FbCommand>();
            _mockReader = new Mock<FbDataReader>();

            // Mock the configuration to provide a connection string
            var mockConfiguration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            mockConfiguration
                .Setup(config => config.GetConnectionString("FirebirdSqlConnection"))
                .Returns("MockConnectionString");

            // Initialize the repository with the mocked configuration
            _repository = new EmployeesRepository(mockConfiguration.Object);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsListOfEmployees()
        {
            // Arrange
            var mockEmployees = new List<Enterprise.Data.Entities.Employees>
            {
                new Enterprise.Data.Entities.Employees { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Mobile = "1234567890", Address = "123 Main St" },
                new Enterprise.Data.Entities.Employees { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Mobile = "9876543210", Address = "456 Elm St" }
            };

            // Mock the data reader behavior
            _mockReader.SetupSequence(reader => reader.ReadAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(true) // First row
                .ReturnsAsync(true) // Second row
                .ReturnsAsync(false); // End of data

            _mockReader.Setup(reader => reader.GetInt32(0)).Returns(1);
            _mockReader.Setup(reader => reader.GetString(1)).Returns("John");
            _mockReader.Setup(reader => reader.GetString(2)).Returns("Doe");
            _mockReader.Setup(reader => reader.GetString(3)).Returns("john.doe@example.com");
            _mockReader.Setup(reader => reader.GetString(4)).Returns("1234567890");
            _mockReader.Setup(reader => reader.GetString(5)).Returns("123 Main St");

            // Mock the command behavior
            _mockCommand.Setup(cmd => cmd.ExecuteReaderAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(_mockReader.Object);

            // Mock the connection behavior
            _mockConnection.Setup(conn => conn.CreateCommand()).Returns(_mockCommand.Object);

            // Act
            var result = await _repository.GetAllEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Enterprise.Data.Entities.Employees>>(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John", result[0].FirstName);
            Assert.Equal("Jane", result[1].FirstName);
        }

        [Fact]
        public async Task GetEmployeeAsync_ReturnsEmployee_WhenEmployeeExists()
        {
            // Arrange
            _mockReader.SetupSequence(reader => reader.ReadAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(true) // First row
                .ReturnsAsync(false); // End of data

            _mockReader.Setup(reader => reader.GetInt32(0)).Returns(1);
            _mockReader.Setup(reader => reader.GetString(1)).Returns("John");
            _mockReader.Setup(reader => reader.GetString(2)).Returns("Doe");
            _mockReader.Setup(reader => reader.GetString(3)).Returns("john.doe@example.com");
            _mockReader.Setup(reader => reader.GetString(4)).Returns("1234567890");
            _mockReader.Setup(reader => reader.GetString(5)).Returns("123 Main St");

            _mockCommand.Setup(cmd => cmd.ExecuteReaderAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(_mockReader.Object);

            _mockConnection.Setup(conn => conn.CreateCommand()).Returns(_mockCommand.Object);

            // Act
            var result = await _repository.GetEmployeeAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Enterprise.Data.Entities.Employees>(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }

        [Fact]
        public async Task SaveEmployeeAsync_ReturnsTrue_WhenEmployeeIsSaved()
        {
            // Arrange
            var newEmployee = new Enterprise.Data.Entities.Employees
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                Mobile = "5555555555",
                Address = "789 Pine St"
            };

            _mockCommand.Setup(cmd => cmd.ExecuteNonQueryAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(1); // Simulate one row affected

            _mockConnection.Setup(conn => conn.CreateCommand()).Returns(_mockCommand.Object);

            // Act
            var result = await _repository.SaveEmployeeAsync(newEmployee);

            // Assert
            Assert.True(result);
        }
    }
}

