using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;
        private readonly ILogger<EmployeesController> _logger;
        public EmployeesController(IEmployeesService employeesService, ILogger<EmployeesController> logger)
        {
            _employeesService = employeesService;
            _logger = logger;
        }

        
        [HttpGet]
        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            _logger.LogInformation("Started calling GetAllEmployee method at {time}", DateTime.UtcNow);
            var data = await _employeesService.GetAllEmployeeAsync();
            _logger.LogInformation("GetAllEmployee called at {time}", DateTime.UtcNow);

            return data;
        }

        [HttpGet("{id}")]
        public async Task<EmployeeDto> GetEmployee(int id)
           => await _employeesService.GetEmployeeAsync(id);

        [HttpPost]
        public async Task<bool> SaveEmployee([FromBody] EmployeeDto employees)
          => await _employeesService.SaveEmployeeAsync(employees);
    }
}
