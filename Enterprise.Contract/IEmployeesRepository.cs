using Enterprise.Data.Dtos;
using Enterprise.Data.Entities;

namespace Enterprise.Contract
{
    public interface IEmployeesRepository
    {
        Task<List<Employees>> GetAllEmployeesAsync();
        Task<Employees> GetEmployeeAsync(int id);
        Task<bool> SaveEmployeeAsync(Employees  employees);
    }
}
