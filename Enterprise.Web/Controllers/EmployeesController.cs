using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet]
        public async Task<List<EmployeeDto>> GetAllEmployees()
           => await _employeesService.GetAllEmployeeAsync();

        [HttpGet("{id}")]
        public async Task<EmployeeDto> GetEmployee(int id)
           => await _employeesService.GetEmployeeAsync(id);

        [HttpPost]
        public async Task<bool> SaveEmployee([FromBody] EmployeeDto employees)
          => await _employeesService.SaveEmployeeAsync(employees);
    }
}
