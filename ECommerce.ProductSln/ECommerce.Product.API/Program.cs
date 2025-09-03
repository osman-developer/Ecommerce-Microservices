using ECommerce.Product.Infrastructure.DependencyInjection;
using ECommerce.Product.Service.DependencyInjection;
using ECommerce.Common.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register infrastructure layer (e.g., DbContext, repositories, serilog)
builder.Services.AddInfrastructureServices(builder.Configuration);

//Register service layer services (e.g., product)
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
app.UseSharedMiddlewares(loggerFactory);

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
