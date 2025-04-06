using Enterprise.Data.Dtos;
using Enterprise.Data.Entities;
using Enterprise.Data.Queries;

namespace Enterprise.Contract
{
    public interface ICustomersService
    {
        Task<CustomerDto> GetAsync(Guid id);
        Task<IEnumerable<CustomerDto>> BrowseAsync(BrowseCustomers query);
    }
}
