using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerService.Filters;
using Entities.Models;
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

namespace CustomerServiceTests
{
    public class ValidateCustomerExistAttributeTests
    {
        private readonly Mock<IRepository<Customer>> _mockRepository = new();
        
        [Fact]
        public async Task NonExistingCustomerId_ReturnsNotFound()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Customer) null);
            var modelState = new ModelStateDictionary();
            var httpContextMock = new DefaultHttpContext();
            httpContextMock.Request.RouteValues.Add("customerId", It.IsAny<Guid>());
            httpContextMock.Items.Add("customerId", It.IsAny<Guid>()); 
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
            actionExecutingContext.ActionArguments.Add("customerId", It.IsAny<Guid>());
            var vceAttribute = new ValidateCustomerExistAttribute(_mockRepository.Object);
            var context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>());

            
            await Assert.ThrowsAsync<NotFoundException>(()=> vceAttribute.OnActionExecutionAsync(actionExecutingContext, async () => { return context; }));

        }
    }
}