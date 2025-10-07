using ECommerce.Authentication.Infrastructure.Data;
using ECommerce.Authentication.Infrastructure.DependencyInjection;
using ECommerce.Authentication.Service.DependencyInjection;
using ECommerce.Common.LogConfiguration;
using ECommerce.Common.Middleware;
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

//Register service layer services (e.g. automapper)
builder.Services.AddServices(builder.Configuration);


// ─── Build App ──────────────────────────────────────────────────────
var app = builder.Build();

// ─── Seed Database ───────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Seed roles and demo users
        await DbSeeder.SeedAsync(services);
        Log.Information("Database seeding completed successfully.");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An error occurred while seeding the database.");
        throw; // Stop app if seeding fails
    }
}

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

app.UseAuthentication();
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