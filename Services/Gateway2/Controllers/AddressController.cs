using System;
using System.Threading.Tasks;
using CommonLib.Helpers.Jwt;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gateway2.Controllers
{
    [Route("customers/{customerId}/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private Service _customerService;

        public AddressController()
        {
            _customerService = new Service("customerservice", 80);
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetCustomerAddress([FromRoute]Guid customerId)
        {
            ValidationControl(customerId); 
            var (response, _) = await _customerService.Route(HttpContext);
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