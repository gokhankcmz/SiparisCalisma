using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CustomerService.HealthChecks
{
    public class ResponseTimeHealthCheck : IHealthCheck
    {
        private Random _rnd = new();
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            int responseTimeInMs = _rnd.Next(1, 300);
            if (responseTimeInMs <100)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"The response time is good: {responseTimeInMs}."));
            }
            if (responseTimeInMs < 200)
            {
                return Task.FromResult(HealthCheckResult.Degraded($"The response time is slow: {responseTimeInMs}."));
            }
            
            return Task.FromResult(HealthCheckResult.Unhealthy($"The response time is unacceptable: {responseTimeInMs}."));
        }
    }

}