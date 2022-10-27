using AcquiringBank.Simulator;
using Checkout.PaymentGateway.Api.Configurations;
using Checkout.PaymentGateway.Api.OperationFilters;
using Checkout.PaymentGateway.Application.Contracts.Commands;
using Checkout.PaymentGateway.Application.EventHandlers;
using Checkout.PaymentGateway.Persistence.Repositories;
using MediatR;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISecurityConfiguration, SecurityConfiguration>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionEventHandler, TransactionEventHandler>();
builder.Services.AddMediatR(typeof(CreateTransactionCommand));
builder.Services.AddScoped<IBankSimulator, BankSimulator>();
builder.Services.AddMemoryCache();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Checkout.com Payment Gateway",
            Version = "v1",
            Description = "An API to facilitate payments from merchant to acquiring bank",
            Contact = new OpenApiContact
            {
                Name = "Checkout.com Payment Gateway",
                Email = "mdeeokoye@gmail.com",
            },
        });

    options.OperationFilter<IdentityHeaderOperationFilter>();
    options.OperationFilter<MacHeaderOperationFilter>();

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
