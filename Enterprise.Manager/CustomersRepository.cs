using Enterprise.Contract;
using Enterprise.Data.Entities;
using Enterprise.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Manager
{
    public class CustomersRepository : ICustomersRepository
    {
        //private readonly IMongoRepository<Customer> _repository;

        //public CustomersRepository(IMongoRepository<Customer> repository)
        //{
        //    _repository = repository;
        //}
        public Task<IEnumerable<Customer>> BrowseAsync(BrowseCustomers id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
