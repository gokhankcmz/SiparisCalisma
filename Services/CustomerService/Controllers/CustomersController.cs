using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.Filters;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CustomerService.Controllers
{
    [Route("customers")]
    [ApiController]
    [ServiceFilter(typeof(ValidateEmailIsUniqueAttribute))]
    public class CustomersController : ControllerBase
    {
        private IApplicationService _applicationService;
        private IMapper _mapper;
        private ILogger _logger;

        public CustomersController(IApplicationService applicationService, IMapper mapper, ILogger logger)
        {
            _applicationService = applicationService;
            _mapper = mapper;
            _logger = logger;
        }


        
        [HttpGet("{customerId:guid}", Name = "CustomerById")]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            var customerEntity = await _applicationService.GetAsync(customerId);
            return Ok(customerEntity);
            //return Ok(_mapper.Map<CustomerResponseDto>(customerEntity));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _applicationService.GetAllAsync();
            return Ok(_mapper.Map<List<CustomerCollectionDto>>(customers));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]CreateCustomerDto createCustomerDto)
        {
            var customerEntity = _mapper.Map<Customer>(createCustomerDto);
            await _applicationService.CreateAsync(customerEntity);
            return CreatedAtRoute("CustomerById", new { customerId = customerEntity.Id },
                _mapper.Map<CustomerResponseDto>(customerEntity));
        }
        
        [HttpPut("{customerId:guid}")]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, [FromBody] UpdateCustomerDto updateCustomerDto)
        {
            var customerEntity = await _applicationService.GetAsync(customerId);
            _mapper.Map(updateCustomerDto, customerEntity);
            await _applicationService.ReplaceAsync(customerEntity);
            return Ok(_mapper.Map<CustomerResponseDto>(customerEntity));
        }


        [HttpDelete("{customerId:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            var customerEntity = await _applicationService.GetAsync(customerId);
            _applicationService.Delete(customerEntity);
            return Ok(_mapper.Map<CustomerResponseDto>(customerEntity));
        }


    }
}