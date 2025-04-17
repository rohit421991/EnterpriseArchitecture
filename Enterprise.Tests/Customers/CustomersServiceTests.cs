using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Data.Entities;
using Enterprise.Manager;
using Moq;

namespace Enterprise.Tests.Customers
{
    public class CustomersServiceTests
    {
        private readonly Mock<ICustomersRepository> _mockCustomersRepository;
        private readonly CustomersService _customersService;

        public CustomersServiceTests()
        {
            _mockCustomersRepository = new Mock<ICustomersRepository>();
            _customersService = new CustomersService(_mockCustomersRepository.Object);
        }

        [Fact]
        public async Task GetAsync_ReturnsCustomerDto_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var mockCustomer = new Customer
            {
                Id = customerId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            _mockCustomersRepository
                .Setup(repo => repo.GetAsync(customerId))
                .ReturnsAsync(mockCustomer);

            // Act
            var result = await _customersService.GetAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CustomerDto>(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("john.doe@example.com", result.Email);

            // Verify that the repository method was called exactly once
            _mockCustomersRepository.Verify(repo => repo.GetAsync(customerId), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ReturnsNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _mockCustomersRepository
                .Setup(repo => repo.GetAsync(customerId))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _customersService.GetAsync(customerId);

            // Assert
            Assert.Null(result);

            // Verify that the repository method was called exactly once
            _mockCustomersRepository.Verify(repo => repo.GetAsync(customerId), Times.Once);
        }
    }
}
