using ECommerce.Common.DelegatingHandlers;
using ECommerce.Common.DependencyInjection;
using ECommerce.Common.LogConfiguration;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ─── Configure Logging ──────────────────────────────────────────────
//serilog registeration
var logFileName = builder.Configuration["SeriLog:FileName"] ?? "Logs/log";
var logger = SerilogConfiguration.ConfigureSerilog(logFileName);
builder.Host.UseSerilog(logger);


// Load Ocelot config
builder.Configuration.AddJsonFile("Config/ocelot.json", optional: false, reloadOnChange: true);

// ─── Services ───────────────────────────────
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

// Add JWT authentication
JwtAuthenticationExtensions.AddJwtAuthentication(builder.Services, builder.Configuration);

builder.Services.AddTransient<AddTrustedHeaderHandler>(sp =>
    new AddTrustedHeaderHandler(sp.GetRequiredService<IConfiguration>(), useInternalApiKey: false));

// Add Ocelot with caching
builder.Services.AddOcelot(builder.Configuration)
                .AddCacheManager(x => x.WithDictionaryHandle())
                .AddDelegatingHandler<AddTrustedHeaderHandler>(true); // every route

var app = builder.Build();

// ─── Middleware Pipeline ───────────────────
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Use Ocelot at the end
await app.UseOcelot();

// ─── Run App ────────────────────────────────────────────────────────
try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush(); // Ensures logs are flushed to files or sinks
}