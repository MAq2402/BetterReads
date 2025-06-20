using System.Security.Claims;
using BetterReads.Shared.Web.ExceptionHandlers;
using BetterReads.Shared.Web.Extensions;
using BetterReads.Shelves.Application.Commands;
using BetterReads.Shelves.Application.Dtos;
using BetterReads.Shelves.Application.Queries;
using BetterReads.Shelves.Infra.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblyContaining<AddShelf>(); });
builder.Services.AddInfra(builder.Configuration);
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapGet("/hello-world", () => "Hello world From Shelves Service")
    .WithName("HelloWorld");

app.MediatorMapPostRequireAuthorization<CreateShelfDto>("/shelves");

app.MediatorMapPostRequireAuthorization<AddBookDto>("/shelves/books");

app.MapGet("/shelves/{id}", async (IMediator mediator, Guid id, ClaimsPrincipal user) => await mediator.Send(
        new GetShelf(user.GetUserId(), id)))
    .WithOpenApi()
    .RequireAuthorization();

app.MapGet("/users/{userId}/shelves", async (IMediator mediator, Guid userId) => await mediator.Send(
        new GetShelves(userId)))
    .WithOpenApi();

app.Run();
public partial class Program { }