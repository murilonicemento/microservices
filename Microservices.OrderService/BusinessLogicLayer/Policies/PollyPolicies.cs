using BusinessLogicLayer.Policies.Contracts;
using Microsoft.Extensions.Logging;
using Polly;

namespace BusinessLogicLayer.Policies;

public class PollyPolicies : IPollyPolicies
{
    private readonly ILogger<UsersMicroservicePolices> _logger;

    public PollyPolicies(ILogger<UsersMicroservicePolices> logger)
    {
        _logger = logger;
    }

    public IAsyncPolicy<HttpResponseMessage> GetRetryPolice(int retryCount)
    {
        var policy = Policy.HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: retryCount,
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

    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolice(int handledEventsAllowedBeforeBreaking,
        TimeSpan durationOfBreak)
    {
        var policy = Policy.HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: handledEventsAllowedBeforeBreaking,
                durationOfBreak: durationOfBreak,
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

    public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeoutDuration)
    {
        var policy = Policy.TimeoutAsync<HttpResponseMessage>(timeoutDuration);

        return policy;
    }
}