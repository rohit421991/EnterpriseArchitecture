using Enterprise.Data.Dtos;
using Enterprise.Data.Entities;

namespace Enterprise.Contract
{
    public interface IEmployeesService
    {
        Task<List<EmployeeDto>> GetAllEmployeeAsync();
        Task<EmployeeDto> GetEmployeeAsync(int id);
        Task<bool> SaveEmployeeAsync(EmployeeDto employees);
    }
}
