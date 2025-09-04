using Serilog;
using Serilog.Core;

namespace ECommerce.Common.LogConfiguration
{
    public static class SerilogConfiguration
    {
        public static Logger ConfigureSerilog(string logFileName)
        {
            //Configre Serilog Logging
            //if u want to use other than Serilog, just change this block and the one in program.cs, everything will be working as i depend on ILogger in code
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(
                    path: $"{logFileName}-.log",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Logger = logger;

            return logger;
        }
    }
}
