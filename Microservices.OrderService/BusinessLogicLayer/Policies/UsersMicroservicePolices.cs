using BusinessLogicLayer.Policies.Contracts;
using Microsoft.Extensions.Logging;
using Polly;

namespace BusinessLogicLayer.Policies;

public class UsersMicroservicePolices : IUsersMicroservicePolicies
{
    private readonly IPollyPolicies _pollyPolicies;

    public UsersMicroservicePolices(IPollyPolicies pollyPolicies)
    {
        _pollyPolicies = pollyPolicies;
    }

    public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        var retryPolicy = _pollyPolicies.GetRetryPolice(5);
        var circuitBreakerPolicy = _pollyPolicies.GetCircuitBreakerPolice(3, TimeSpan.FromMinutes(2));
        var timeoutPolicy = _pollyPolicies.GetTimeoutPolicy(TimeSpan.FromSeconds(5));

        return Policy.WrapAsync(
            retryPolicy,
            circuitBreakerPolicy,
            timeoutPolicy
        );
    }
}