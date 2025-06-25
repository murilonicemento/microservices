using Polly;

namespace BusinessLogicLayer.Policies.Contracts;

public interface IPollyPolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetRetryPolice(int retryCount);

    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolice(
        int handledEventsAllowedBeforeBreaking,
        TimeSpan durationOfBreak
    );

    public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeoutDuration);
}