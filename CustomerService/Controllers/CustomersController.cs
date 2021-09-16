using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.Filters;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace CustomerService.Controllers
{
    [Route("customers")]
    [ApiController]
    
    public class CustomersController : ControllerBase
    {
        private IRepository<Customer> _repository;
        private IMapper _mapper;

        public CustomersController(IRepository<Customer> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]CreateCustomerDto createCustomerDto)
        {
            var customerEntity = (await _repository.GetByCondition(x=> x.Email==createCustomerDto.Email)).FirstOrDefault();
            if (customerEntity != null)
            {
                throw new EmailIsNotUniqueException(nameof(Customer), createCustomerDto.Email);
            }
            
            customerEntity = _mapper.Map<Customer>(createCustomerDto);
            await _repository.CreateAsync(customerEntity);
            return CreatedAtRoute("CustomerById", new { customerId = customerEntity.Id },
                 _mapper.Map<CustomerResponseDto>(customerEntity));
        }

        
        [HttpGet("{customerId:guid}", Name = "CustomerById")]
        [ServiceFilter(typeof(ValidateCustomerExistAttribute))]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            var customerEntity = await _repository.GetByIdAsync(customerId);
            return Ok(customerEntity);
            //return Ok(_mapper.Map<CustomerResponseDto>(customerEntity));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _repository.GetAll();
            return Ok(_mapper.Map<List<CustomerCollectionDto>>(customers));
        }

        
        [HttpPut("{customerId:guid}")]
        [ServiceFilter(typeof(ValidateCustomerExistAttribute))]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, [FromBody] UpdateCustomerDto updateCustomerDto)
        {
            var customerEntity = (await _repository.GetByCondition(x=> x.Email==updateCustomerDto.Email)).FirstOrDefault();
            if (customerEntity != null)
            {
                throw new EmailIsNotUniqueException(nameof(Customer), updateCustomerDto.Email);
            }
            
            customerEntity = await _repository.GetByIdAsync(customerId);
            _mapper.Map(updateCustomerDto, customerEntity);
            await _repository.ReplaceAsync(customerEntity);
            return Ok(_mapper.Map<CustomerResponseDto>(customerEntity));
        }


        [HttpDelete("{customerId:guid}")]
        [ServiceFilter(typeof(ValidateCustomerExistAttribute))]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            var customerEntity = await _repository.GetByIdAsync(customerId);
            _repository.Delete(customerEntity);
            return Ok(_mapper.Map<CustomerResponseDto>(customerEntity));
        }


    }
}