using BusinessLogicLayer.Policies.Contracts;
using Microsoft.Extensions.Logging;
using Polly;

namespace BusinessLogicLayer.Policies;

public class UsersMicroservicePolices : IUsersMicroservicePolicies
{
    private readonly ILogger<UsersMicroservicePolices> _logger;

    public UsersMicroservicePolices(ILogger<UsersMicroservicePolices> logger)
    {
        _logger = logger;
    }

    public IAsyncPolicy<HttpResponseMessage> GetRetryPolice()
    {
        var policy = Policy.HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timeSpan, retryAttempt, context) =>
                {
                    _logger.LogInformation(
                        "Retry {RetryAttempt} after {TimeSpanTotalSeconds} seconds", retryAttempt,
                        timeSpan.TotalSeconds);
                }
            );

        return policy;
    }

    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolice()
    {
        var policy = Policy.HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromMinutes(2),
                onBreak: (outcome, timespan) =>
                {
                    _logger.LogInformation(
                        "Circuit breaker opened for {TimespanTotalMinutes} minutes due to consecutive 3 failures. The subsequent will be blocked."
                        , timespan.TotalMinutes);
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker closed. The subsequent requests will be allowed.");
                }
            );

        return policy;
    }

    public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        var policy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(1500));

        return policy;
    }

    public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        var retryPolicy = GetRetryPolice();
        var circuitBreakerPolicy = GetCircuitBreakerPolice();
        var timeoutPolicy = GetTimeoutPolicy();

        return Policy.WrapAsync(
            retryPolicy,
            circuitBreakerPolicy,
            timeoutPolicy
        );
    }
}