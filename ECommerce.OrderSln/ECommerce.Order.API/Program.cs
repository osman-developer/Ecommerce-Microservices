using ECommerce.Common.LogConfiguration;
using ECommerce.Common.Middleware;
using ECommerce.Order.Infrastructure.DependencyInjection;
using ECommerce.Order.Service.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// ─── Configure Logging ──────────────────────────────────────────────
//serilog registeration
var logFileName = builder.Configuration["SeriLog:FileName"] ?? "Logs/log";
var logger = SerilogConfiguration.ConfigureSerilog(logFileName);
builder.Host.UseSerilog(logger);


// ─── DI Register Services ──────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Register infrastructure layer (e.g., DbContext,JWTAuth repositories)
builder.Services.AddInfrastructureServices(builder.Configuration);

//Register service layer services (e.g., order, automapper)
builder.Services.AddServices(builder.Configuration);


// ─── Build App ──────────────────────────────────────────────────────
var app = builder.Build();


// ─── Middleware Pipeline ──────────────────────────────────────────── 
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


// ─── Run App ────────────────────────────────────────────────────────
try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush(); // Ensures logs are flushed to files or sinks
}