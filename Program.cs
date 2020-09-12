using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using SalesProcessor.Infrastructure.Configuration.Logging;
using Serilog;
using Serilog.Formatting.Compact;


namespace SalesProcessor
{
    class Program
    {
        public static IConfigurationRoot _configuration;
        private static ILogger _logger;
        private static string _homepath;

        static void Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            //Suport for Windows or other systems
            var envHome = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "HOMEPATH" : "HOME";
            _homepath = Environment.GetEnvironmentVariable(envHome);

            _logger = ConfigureLogger();
            _logger.Information("Logger configured!");
        }

        private static ILogger ConfigureLogger()
        {
            //To follow the requested pattern, all directories will be inside the %HOMEPATH%
            var loggerConfiguration = _configuration.GetSection("LoggingSettings").Get<LoggingSettings>();
            var logFileOutPath = (_homepath + '\\' + loggerConfiguration.logFileOutput);

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.RollingFile(new CompactJsonFormatter(), logFileOutPath)
                .CreateLogger();
        }
    }
}
