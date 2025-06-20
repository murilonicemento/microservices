﻿using Polly;

namespace BusinessLogicLayer.Policies;

public interface IUsersMicroservicePolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetRetryPolice();
}