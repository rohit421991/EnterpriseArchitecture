using Enterprise.Contract;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Enterprise.Tests.Customers
{
    public class CustomersRepositoryTests
    {
        private readonly ICustomersRepository _repository;

        public CustomersRepositoryTests()
        {
            _repository = Substitute.For<ICustomersRepository>();
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            var result = await _repository.GetAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_Has_Failed()
        {
            var val = _repository.GetAsync(Arg.Any<Guid>()).Throws(new Exception("Error in calling DB", new Exception()));

            var value = await Assert.ThrowsAsync<Exception>(
                async () => await _repository.GetAsync(Guid.NewGuid()));
           
            Assert.True(value.Message == "Error in calling DB");
        }
    }
}

