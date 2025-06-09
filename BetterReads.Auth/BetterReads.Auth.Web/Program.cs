using BetterReads.Auth.Application.Commands;
using BetterReads.Auth.Application.Dtos;
using BetterReads.Auth.Application.Queries;
using BetterReads.Auth.Infra.Extensions;
using BetterReads.Shared.Web.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddInfra(builder.Configuration);
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblyContaining<LoginQuery>();
});

builder.Services.AddMassTransit(x =>
{
    x.UsingAzureServiceBus((context,cfg) =>
    {
        cfg.Host(builder.Configuration.GetSection("AzureServiceBus").GetValue<string>("ConnectionString"));

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hello-world", () => "Hello world From Auth Service")
    .WithName("HelloWorld");

app.MediatorMapGet<LoginQuery, LoginResponse>("/login/{code}");
app.MediatorMapPost<RegisterCommand>("/register");

app.Run();