using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesProcessor.Infrastructure.Configuration.FileWatcher;
using SalesProcessor.Infrastructure.Configuration.Logging;
using SalesProcessor.Infrastructure.FileWatcher;
using Serilog;
using Serilog.Formatting.Compact;


namespace SalesProcessor
{
    class Program
    {
        public static IConfigurationRoot _configuration;
        private static ILogger _logger;
        private static IServiceProvider _services;

        static void Main(string[] args)
        {
            Configure();

            //Monitor folder to see if has something to process
            _services.GetService<IFileWatcher>().watch();

            //Don`t stop until the user request
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        private static void Configure(){

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _logger = ConfigureLogger();
            _logger.Information("Logger configured!");

            _services = ConfigureServices();
        }
        private static ILogger ConfigureLogger()
        {
            //To follow the requested pattern, all directories will be inside the %HOMEPATH%
            var loggerConfiguration = _configuration.GetSection("LoggingSettings").Get<LoggingSettings>();
            var logFileOutPath = (loggerConfiguration.homePath + '\\' + loggerConfiguration.logFileOutput);

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.RollingFile(new CompactJsonFormatter(), logFileOutPath)
                .CreateLogger();
        }

        private static IServiceProvider ConfigureServices(){
            var fileWatcherConfiguration = _configuration.GetSection("FileWatcherSettings").Get<FileWatcherSettings>();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IFileWatcher>(new FileWatcher(_logger, fileWatcherConfiguration))
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
