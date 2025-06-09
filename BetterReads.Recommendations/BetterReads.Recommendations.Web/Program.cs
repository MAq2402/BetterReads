using System.Security.Claims;
using BetterReads.Recommendations.Application.Commands;
using BetterReads.Recommendations.Application.Consumers;
using BetterReads.Recommendations.Application.Queries;
using BetterReads.Recommendations.Application.Services;
using BetterReads.Recommendations.Infra.Clients;
using BetterReads.Recommendations.Infra.Extensions;
using BetterReads.Recommendations.Infra.Services;
using BetterReads.Recommendations.Infra.Settings;
using BetterReads.Shared.Application.Commands;
using BetterReads.Shared.Application.Events;
using BetterReads.Shared.Web.Extensions;
using MassTransit;
using MediatR;
using Microsoft.SemanticKernel;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfra(builder.Configuration);

builder.Services.Configure<AzureOpenAiSettings>(builder.Configuration.GetSection("AzureOpenAi"));

var openAiSettings = builder.Configuration.GetSection("AzureOpenAi").Get<AzureOpenAiSettings>()
                     ?? throw new NullReferenceException(nameof(AzureOpenAiSettings));
builder.Services.AddAzureOpenAIChatCompletion(
    deploymentName: openAiSettings.Deployment,
    endpoint: openAiSettings.Endpoint,
    apiKey: openAiSettings.ApiKey,
    modelId: openAiSettings.ModelId);
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblyContaining<GetRecommendations>(); });
builder.Services.AddTransient<IAiService, AzureOpenAiService>();
builder.Services.Configure<ShelvesClientSettings>(builder.Configuration.GetSection("Shelves"));
builder.Services.AddHttpClient<ShelvesService>();
builder.Services.AddScoped<IShelvesService, ShelvesService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BookAddedConsumer>();
    x.AddConsumer<AddInitialRecommendationsConsumer>();
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetSection("AzureServiceBus").GetValue<string>("ConnectionString"));

        cfg.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(AddInitialRecommendations)),
            e => e.ConfigureConsumer<AddInitialRecommendationsConsumer>(context));
        cfg.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(BookAdded)), e => e.ConfigureConsumer<BookAddedConsumer>(context));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.MapPut("/recommendations", async (IMediator mediator, ClaimsPrincipal user) => await mediator.Send(
        new UpdateRecommendations(user.GetUserId())))
    .WithOpenApi()
    .RequireAuthorization();

app.MapGet("/recommendations", async (IMediator mediator, ClaimsPrincipal user) => await mediator.Send(
        new GetRecommendations(user.GetUserId())))
    .WithOpenApi()
    .RequireAuthorization();


app.Run();