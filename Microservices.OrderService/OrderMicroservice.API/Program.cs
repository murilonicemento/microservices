using BusinessLogicLayer;
using BusinessLogicLayer.HttpClients;
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

builder.Services.AddTransient<IUsersMicroservicePolicies, UsersMicroservicePolice>();

var usersMicroservicePolicy = builder.Services.BuildServiceProvider().GetRequiredService<IUsersMicroservicePolicies>()
    .GetRetryPolice();

builder.Services.AddHttpClient<UserMicroserviceClient>(client =>
    {
        client.BaseAddress =
            new Uri(
                $"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");
    })
    .AddPolicyHandler(usersMicroservicePolicy);

builder.Services.AddHttpClient<ProductMicroserviceClient>(client =>
{
    client.BaseAddress =
        new Uri(
            $"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");
});

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