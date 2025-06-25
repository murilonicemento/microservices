using BusinessLogicLayer;
using BusinessLogicLayer.HttpClients;
using BusinessLogicLayer.Policies.Contracts;
using BusinessLogicLayer.Policies;
using DataAccessLayer;
using FluentValidation.AspNetCore;
using OrderMicroservice.API.Middlewares;
using Polly;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddDataAccessLayer(configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(temp =>
    {
        temp.WithOrigins("http;//localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddTransient<IUsersMicroservicePolicies, UsersMicroservicePolices>();
builder.Services.AddTransient<IProductsMicroservicePolicies, ProductsMicroservicePolicies>();

var builderProvider = builder.Services.BuildServiceProvider();

var usersMicroserviceRetryPolicy = builderProvider
    .GetRequiredService<IUsersMicroservicePolicies>()
    .GetRetryPolice();
var usersMicroserviceCircuitBreakerPolicy = builderProvider
    .GetRequiredService<IUsersMicroservicePolicies>()
    .GetCircuitBreakerPolice();
var usersMicroserviceTimeoutPolicy = builderProvider
    .GetRequiredService<IUsersMicroservicePolicies>()
    .GetTimeoutPolicy();
var productsMicroserviceFallbackPolicy = builderProvider
    .GetRequiredService<IProductsMicroservicePolicies>()
    .GetFallbackPolicy();
var productsMicroserviceBulkheadIsolation = builderProvider
    .GetRequiredService<IProductsMicroservicePolicies>()
    .GetBulkheadIsolationPolicy();

builder.Services.AddHttpClient<UserMicroserviceClient>(client =>
    {
        client.BaseAddress =
            new Uri(
                $"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");
    })
    .AddPolicyHandler(usersMicroserviceRetryPolicy)
    .AddPolicyHandler(usersMicroserviceCircuitBreakerPolicy)
    .AddPolicyHandler(usersMicroserviceTimeoutPolicy);

builder.Services.AddHttpClient<ProductMicroserviceClient>(client =>
    {
        client.BaseAddress =
            new Uri(
                $"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");
    })
    .AddPolicyHandler(productsMicroserviceFallbackPolicy)
    .AddPolicyHandler(productsMicroserviceBulkheadIsolation);

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();