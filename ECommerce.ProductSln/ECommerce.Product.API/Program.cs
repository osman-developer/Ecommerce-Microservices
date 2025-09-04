using ECommerce.Product.Infrastructure.DependencyInjection;
using ECommerce.Product.Service.DependencyInjection;
using ECommerce.Common.Middleware;
using Serilog;
using ECommerce.Common.LogConfiguration;

var builder = WebApplication.CreateBuilder(args);


//serilog registeration
var logFileName = builder.Configuration.GetSection("SeriLog")["FileName"] ?? "Logs/log"; ;
var logger = SerilogConfiguration.ConfigureSerilog(logFileName);
builder.Host.UseSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register infrastructure layer (e.g., DbContext,JWTAuth repositories)
builder.Services.AddInfrastructureServices(builder.Configuration);

//Register service layer services (e.g., product, automapper)
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

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush(); // Ensures logs are flushed to files or sinks
}