using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CommonLib.z_old_files;
using CustomerService;
using CustomerService.Filters;
using Entities.Models;
using Entities.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace CustomerServiceTests.FilterTests
{
    public class ValidateEmailIsUniqueAttributeTests
    {
        private readonly Mock<IApplicationService> _mockApplicationService = new();
        
        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        public async Task RegisteredEmail_ReturnsEmailNotUniqueException(string requestMethod)
        {
            _mockApplicationService.Setup(x => x.GetByConditionAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
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
            
            var attribute = new ValidateEmailIsUniqueAttribute(_mockApplicationService.Object);
            var context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>());

            
            await Assert.ThrowsAsync<EmailIsNotUniqueException>(()=> attribute.OnActionExecutionAsync(actionExecutingContext, async () => { return context; }));

        }
    }
}