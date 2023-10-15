using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BlazorDemo.HealthCheck;

public class DatabaseHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();

    }
}
