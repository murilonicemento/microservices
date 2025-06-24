using Polly;

namespace BusinessLogicLayer.Policies.Contracts;

public interface IProductsMicroservicePolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy();
}