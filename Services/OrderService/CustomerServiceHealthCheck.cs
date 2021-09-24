using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OrderService
{
    public class CustomerServiceHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            
            HttpClient client = new HttpClient();
            var res =  await client.GetAsync("http://customerservice:80/health");
            if (res.StatusCode == HttpStatusCode.OK)
            {
                return HealthCheckResult.Healthy();
            }
            return HealthCheckResult.Unhealthy();
        }
    }
}