using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using OrderService.Filters;
using Repository;
using Xunit;

namespace OrderServiceTests
{
    public class ValidateOrderExistAttributeTests
    {
        
        private readonly Mock<IRepository<Order>> _mockRepository = new();
        
        [Fact]
        public async Task NonExistingOrderId_ReturnsNotFound()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Order) null);
            var modelState = new ModelStateDictionary();
            var httpContextMock = new DefaultHttpContext();
            httpContextMock.Items.Add("orderId", It.IsAny<Guid>()); 
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
            actionExecutingContext.ActionArguments.Add("orderId", It.IsAny<Guid>());
            var vceAttribute = new ValidateOrderExistAttribute(_mockRepository.Object);
            var context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>());

            
            await Assert.ThrowsAsync<NotFoundException>(()=> vceAttribute.OnActionExecutionAsync(actionExecutingContext, async () => { return context; }));

        }
    }
}