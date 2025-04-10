using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Data.Entities;

namespace Enterprise.Manager
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;

        public EmployeesService(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<List<EmployeeDto>> GetAllEmployeeAsync()
        {
            var users = await _employeesRepository.GetAllEmployeesAsync();
            //use mapper
            return users.Select(user => new EmployeeDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Address = user.Address
            }).ToList();
        }

        public async Task<EmployeeDto?> GetEmployeeAsync(int id)
        {
            var user = await _employeesRepository.GetEmployeeAsync(id);

            //use mapper
            return user == null ? null : new EmployeeDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Address = user.Address,
            };
        }

        public async Task<bool> SaveEmployeeAsync(EmployeeDto employees)
        {
            Employees modal = new Employees
            {
                Id = employees.Id,
                Email = employees.Email,
                FirstName = employees.FirstName,
                LastName = employees.LastName,
                Mobile = employees.Mobile,
                Address = employees.Address,
            };

            return await _employeesRepository.SaveEmployeeAsync(modal);
        }
    }
}
