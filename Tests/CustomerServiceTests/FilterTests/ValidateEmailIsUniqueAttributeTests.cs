using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CustomerService.Filters;
using Entities.Models;
using Entities.RequestModels;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Repository;
using Xunit;

namespace CustomerServiceTests.FilterTests
{
    public class ValidateEmailIsUniqueAttributeTests
    {
        private readonly Mock<IRepository<Customer>> _mockRepository = new();
        
        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        public async Task RegisteredEmail_ReturnsEmailNotUniqueException(string requestMethod)
        {
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(new List<Customer> {new ()});
            var modelState = new ModelStateDictionary();
            var httpContextMock = new DefaultHttpContext();
            httpContextMock.Request.Method = requestMethod;
            
            var actionContext = new ActionContext(
                httpContextMock,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState
            );

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );
            if(requestMethod.Equals("POST")) actionExecutingContext.ActionArguments.Add("createCustomerDto",Mock.Of<CreateCustomerDto>());
            if(requestMethod.Equals("PUT")) actionExecutingContext.ActionArguments.Add("updateCustomerDto",Mock.Of<UpdateCustomerDto>());
            
            var attribute = new ValidateEmailIsUniqueAttribute(_mockRepository.Object);
            var context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>());

            
            await Assert.ThrowsAsync<EmailIsNotUniqueException>(()=> attribute.OnActionExecutionAsync(actionExecutingContext, async () => { return context; }));

        }
    }
}