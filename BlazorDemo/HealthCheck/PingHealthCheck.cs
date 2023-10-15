using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace BlazorDemo.HealthCheck;

public class PingHealthCheck : IHealthCheck
{
    private readonly string _host;
    private readonly int _timeout;
    private readonly int _pingInterval;
    private DateTime _lastPingtime = DateTime.MinValue;
    private HealthCheckResult _lastPingResult = HealthCheckResult.Healthy();
    public PingHealthCheck(string host, int timeout, int pingInterval = 0)
    {
        _host = host;
        _timeout = timeout;
        _pingInterval = pingInterval;

    }
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        if (_pingInterval !=0 && _lastPingtime.AddSeconds(_pingInterval) > DateTime.Now)
        {
            return _lastPingResult;
        }

        try
        {
            using (var ping = new Ping())
            {
                _lastPingtime = DateTime.Now;

                var reply = await ping.SendPingAsync(_host, _timeout);
                
                if (reply.Status != IPStatus.Success)
                {
                    return HealthCheckResult.Unhealthy();
                }
                
                if (reply.RoundtripTime >= _timeout)
                {
                    return HealthCheckResult.Degraded();
                }
                return HealthCheckResult.Healthy();
            }
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(exception: exception);
        }

    }
}
