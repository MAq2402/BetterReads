using BetterReads.Books.Application.Models;
using BetterReads.Books.Application.Queries;
using BetterReads.Books.Infra.Extensions;
using BetterReads.Shared.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfra(builder.Configuration);
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblyContaining<SearchBooks>(); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/hello-world", () => "Hello world From Books Service")
    .WithName("HelloWorld");
app.MediatorMapGet<SearchBooks, List<Book>>("/books");


app.Run();