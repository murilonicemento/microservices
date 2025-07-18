using System.Text.Json.Serialization;
using BusinessLogicLayer;
using DataAccessLayer;
using FluentValidation.AspNetCore;
using ProductService.API.ApiEndpoints;
using ProductService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDataAccessLayer(configuration);
builder.Services.AddBusinessLogicLayer(configuration);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapProductApiEndpoints();

app.Run();