using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Data.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Web.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerDto>> Get([FromQuery] BrowseCustomers query)
            => await _customersService.BrowseAsync(query);

        [HttpGet("{guid}")]
        public async Task<CustomerDto> GetCustomers(Guid guid)
            => await _customersService.GetAsync(guid);
    }
}
