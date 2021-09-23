using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestModels;
using MongoDB.Driver;

namespace OrderService
{
    public interface IApplicationService
    {
        Task<Order> GetAsync(Guid guid);
        Task<Order> CreateAsync(Order document);
        Task<Order> ReplaceAsync(Order document);
        DeleteResult Delete(Order document);
        Task<List<Order>> GetByConditionAsync(Expression<Func<Order, bool>> expression);
        Task<List<Order>> GetAllAsync();

        Task<string> GetToken(AuthDto authDto);

        string ReadIdFromToken(string token);
    }
}