using System;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib.Helpers.Jwt;
using CommonLib.Models.ErrorModels;
using CustomerService.Filters;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [Route("customers/{customerId}/address")]
    [ApiController]
    [ServiceFilter(typeof(ValidateCustomerExistAttribute))]
    public class AddressController : ControllerBase
    {
        
        private IApplicationService _applicationService;

        public AddressController(IApplicationService applicationService, IMapper mapper)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerAddress([FromRoute]Guid customerId)
        {
            var idFromToken = Request.Headers.GetClaimOrThrow("nameid");
            if (!idFromToken.Equals(customerId.ToString()))
            {
                throw new UnAuthorized(nameof(Customer), customerId.ToString());
            }
            var customer = await _applicationService.GetAsync(customerId);
            var address = customer.Address;
            return Ok(address);
        }
    }
}