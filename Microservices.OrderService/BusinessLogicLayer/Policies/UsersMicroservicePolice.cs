using Microsoft.Extensions.Logging;
using Polly;

namespace BusinessLogicLayer.Policies;

public class UsersMicroservicePolice : IUsersMicroservicePolicies
{
    private readonly ILogger<UsersMicroservicePolice> _logger;

    public UsersMicroservicePolice(ILogger<UsersMicroservicePolice> logger)
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
                    _logger.LogInformation($"Retry {retryAttempt} after {timeSpan.TotalSeconds} seconds");
                }
            );

        return policy;
    }
}