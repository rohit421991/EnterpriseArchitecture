using Enterprise.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enterprise.Data.Entities;

namespace Enterprise.Contract
{
    public interface ICustomersRepository
    {
        Task<Customer> GetAsync(Guid id);
        Task<IEnumerable<Customer>> BrowseAsync(BrowseCustomers id);
    }
}
