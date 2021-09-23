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

namespace OrderService
{
    public class ApplicationService : IApplicationService 
    {
        private IRepository<Order> _repository;
        private AuthenticationManager<Order> _authManager;

        public ApplicationService(IRepository<Order> repository, AuthenticationManager<Order> authManager)
        {
            _repository = repository;
            _authManager = authManager;
        }

        public async Task<Order> GetAsync(Guid guid)
        {
            return await _repository.GetByIdAsync(guid);
        }

        public async Task<Order> CreateAsync(Order document)
        {
            return await _repository.CreateAsync(document);
        }

        public async Task<Order> ReplaceAsync(Order document)
        {
            return await  _repository.ReplaceAsync(document);
        }

        public DeleteResult Delete(Order document)
        {
            return _repository.Delete(document);
        }

        public async Task<List<Order>> GetByConditionAsync(Expression<Func<Order, bool>> expression)
        {
            return await _repository.GetByCondition(expression);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<string> GetToken(AuthDto authDto)
        {
            var order = (await _repository.GetByCondition(x => x.Id.Equals(authDto.Id))).FirstOrDefault();
            if (order == null)
            {
                throw new Conflict();
            }
            var token = _authManager.Authenticate(order);
            return token;
        }

        public string ReadIdFromToken(string token)
        {
            return _authManager.ReadIdFromToken(token.Replace("Bearer ",""));
        }
    }
}