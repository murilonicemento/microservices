using Polly;

namespace BusinessLogicLayer.Policies.Contracts;

public interface IUsersMicroservicePolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetRetryPolice();
    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolice();

    public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy();

    public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
}