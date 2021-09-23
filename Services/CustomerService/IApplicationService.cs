using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestModels;
using MongoDB.Driver;

namespace CustomerService
{
    public interface IApplicationService
    {
        Task<Customer> GetAsync(Guid guid);
        Task<Customer> CreateAsync(Customer document);
        Task<Customer> ReplaceAsync(Customer document);
        DeleteResult Delete(Customer document);
        Task<List<Customer>> GetByConditionAsync(Expression<Func<Customer, bool>> expression);
        Task<List<Customer>> GetAllAsync();

        Task<string> GetToken(AuthDto authDto);

        string ReadIdFromToken(string token);
    }
}