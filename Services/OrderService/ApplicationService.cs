using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using MongoDB.Driver;
using Repository;

namespace OrderService
{
    public class ApplicationService : IApplicationService 
    {
        private IRepository<Order> _repository;

        public ApplicationService(IRepository<Order> repository)
        {
            _repository = repository;
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
            return await _repository.GetByConditionAsync(expression);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }


        public void CheckIfSelfOrder(Order order, Guid customerId)
        {
            if (!customerId.Equals(order.CustomerId))
            {
                throw new UnAuthorized();
            }
        }


    }
}