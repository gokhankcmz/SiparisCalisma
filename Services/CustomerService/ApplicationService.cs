using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CommonLib.Helpers.Jwt;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using Entities.RequestModels;
using MongoDB.Driver;
using Repository;

namespace CustomerService
{
    public class ApplicationService : IApplicationService 
    {
        private IRepository<Customer> _repository;
        private AuthenticationManager<Customer> _authManager;

        public ApplicationService(IRepository<Customer> repository, AuthenticationManager<Customer> authManager)
        {
            _repository = repository;
            _authManager = authManager;
        }

        public async Task<Customer> GetAsync(Guid guid)
        {
            return await _repository.GetByIdAsync(guid);
        }

        public async Task<Customer> CreateAsync(Customer document)
        {
            return await _repository.CreateAsync(document);
        }

        public async Task<Customer> ReplaceAsync(Customer document)
        {
            return await  _repository.ReplaceAsync(document);
        }

        public DeleteResult Delete(Customer document)
        {
            return _repository.Delete(document);
        }

        public async Task<List<Customer>> GetByConditionAsync(Expression<Func<Customer, bool>> expression)
        {
            return await _repository.GetByConditionAsync(expression);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<string> CreateToken(AuthDto authDto)
        {
            var customer = (await _repository.GetByConditionAsync(x => x.Id.Equals(authDto.Id) && x.Email.Equals(authDto.email))).FirstOrDefault();
            if (customer == null)
            {
                throw new Conflict();
            }
            var token = _authManager.Authenticate(customer);
            return token;
        }

    }
}