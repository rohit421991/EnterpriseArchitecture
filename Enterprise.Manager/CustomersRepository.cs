using Enterprise.Contract;
using Enterprise.Data.Entities;
using Enterprise.Data.Queries;
using Enterprise.Manager.EnterpriseDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer = Enterprise.Data.Entities.Customer;

namespace Enterprise.Manager
{
    public class CustomersRepository : ICustomersRepository
    {
        EnterpriseContext context = new EnterpriseContext();

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
            try
            {
                var data = context.Customers.Where(u => u.Id == id)
                    .Select(x => new Customer
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email
                    }).FirstOrDefaultAsync();

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
