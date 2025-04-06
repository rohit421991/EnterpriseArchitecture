using Enterprise.Contract;
using Enterprise.Data.Entities;
using Enterprise.Data.Queries;
using Enterprise.Manager.EnterpriseDB;
using FirebirdSql.Data.FirebirdClient;
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
        private readonly EnterpriseContext _context;
        public CustomersRepository(EnterpriseContext context)
        {
            _context = context;
        }
        public Task<IEnumerable<Customer>> BrowseAsync(BrowseCustomers id)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> GetAsync(Guid id)
        {
            try
            {
                var data = await _context.Customers.Where(u => u.Id == id)
                    .Select(x => new Customer
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email
                    }).FirstOrDefaultAsync();

                return data;
            }
            catch (Exception) { throw; }
        }
    }
}
