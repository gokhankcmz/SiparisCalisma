using System;
using System.Threading.Tasks;
using CommonLib.Helpers.Jwt;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using Entities.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Gateway2.Controllers
{
    [Route("customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private Service _customerService;
        private ILogger _logger;

        public CustomersController(ILogger logger)
        {
            _customerService = new Service("customerservice", 80);
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var (response, statusCode) = await _customerService.Route(HttpContext);
            return Ok(response);
        }

        [HttpGet("{customerId:guid}", Name = "GetById")]
        public async Task<IActionResult> GetACustomer(Guid customerId)
        {
            var (response, statusCode) = await _customerService.Route(HttpContext);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]CreateCustomerDto createCustomerDto )
        {
            var (response, statusCode) = await _customerService.Route(HttpContext, bodyArg:createCustomerDto);
            return Ok(response);
        }
        
                
        [HttpPut("{customerId:guid}")]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, [FromBody] UpdateCustomerDto updateCustomerDto)
        {
            ValidationControl(customerId); 
            var (response, statusCode) = await _customerService.Route(HttpContext, bodyArg:updateCustomerDto);
            return Ok(response);
        }


        [HttpDelete("{customerId:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            ValidationControl(customerId); 
            var (response, statusCode) = await _customerService.Route(HttpContext);
            return Ok(response);
        }

        private void ValidationControl(Guid customerId)
        {
            var idFromToken = Request.Headers.GetClaimOrThrow("nameid");
            if (!idFromToken.Equals(customerId.ToString()))
            {
                throw new UnAuthorized(nameof(Customer), customerId.ToString());
            }
        }
    }
}