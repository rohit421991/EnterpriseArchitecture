using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Data.Queries;

namespace Enterprise.Manager
{
    public class CustomersService: ICustomersService
    {
        private readonly ICustomersRepository _customersRepository;

        public CustomersService(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        public async Task<CustomerDto?> GetAsync(Guid id)
        {
            var customer = await _customersRepository.GetAsync(id);

            //use mapper
            return customer == null ? null : new CustomerDto
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
        }

        public async Task<IEnumerable<CustomerDto>> BrowseAsync(BrowseCustomers query)
        {
            var pagedResult = await _customersRepository.BrowseAsync(query);

            // use mapper
            var customers = pagedResult.Select(c => new CustomerDto
            {
                Id = c.Id,
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName
            });

            return customers;
        }
    }
}
