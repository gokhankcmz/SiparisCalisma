using System;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.Filters;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace CustomerService.Controllers
{
    [Route("customers/{customerId}/address")]
    [ApiController]
    [ServiceFilter(typeof(ValidateCustomerExistAttribute))]
    public class AddressController : ControllerBase
    {
        
        private IRepository<Customer> _repository;

        public AddressController(IRepository<Customer> repository, IMapper mapper)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerAddress(Guid customerId)
        {
            var customer = await _repository.GetByIdAsync(customerId);
            var address = customer.Address;
            return Ok(address);
        }
    }
}