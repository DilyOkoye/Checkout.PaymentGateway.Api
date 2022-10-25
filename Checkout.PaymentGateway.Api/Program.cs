using Checkout.PaymentGateway.Api.Configurations;
using Checkout.PaymentGateway.Persistence.Repositories;
using Microsoft.Extensions.Options;
using LanguageExt;
using MediatR;
using System.Reflection;
using Checkout.PaymentGateway.Application.Contracts.Commands;
using AcquiringBank.Simulator;
using Checkout.PaymentGateway.Application.EventHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISecurityConfiguration, SecurityConfiguration>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionEventHandler, TransactionEventHandler>();
builder.Services.AddMediatR(typeof(CreateTransactionCommand));
builder.Services.AddScoped<IBankSimulator, BankSimulator>();
builder.Services.AddMemoryCache();
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
