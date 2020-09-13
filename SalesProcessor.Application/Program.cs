using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesProcessor.Domain.LotAnalyzer;
using SalesProcessor.Domain.FileWatcher;
using SalesProcessor.Infrastructure.Configuration.FileWatcher;
using SalesProcessor.Infrastructure.Configuration.Lot;
using SalesProcessor.Infrastructure.Configuration.Logging;
using SalesProcessor.Infrastructure.StreamReader;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using SalesProcessor.Domain.ReportGenerator;

namespace SalesProcessor.Application
{
    public class Program
    {
        public static IConfigurationRoot _configuration;
        public static ILogger _logger;
        public static IServiceProvider _services;

        public static void Main(string[] args)
        {
            Configure();

            //Monitor folder to see if has something to process
            _services.GetService<IFileWatcher>().Watch();

            //Don`t stop until the user request
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        public static void Configure(){

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _logger = ConfigureLogger();

            _services = ConfigureServices();
        }
        public static ILogger ConfigureLogger()
        {
            //To follow the requested pattern, all directories will be inside the %HOMEPATH%
            var loggerConfiguration = _configuration.GetSection("LoggingSettings").Get<LoggingSettings>();
            var logFileOutPath = (loggerConfiguration.homePath + '\\' + loggerConfiguration.logFileOutput);

            LogEventLevel logLevel;
            switch (loggerConfiguration.logLevel)
            {
                case "debug":
                    logLevel = LogEventLevel.Debug;
                    break;
                case "information":
                    logLevel = LogEventLevel.Information;
                    break;
                case "warning":
                    logLevel = LogEventLevel.Warning;
                    break;
                case "error":
                    logLevel = LogEventLevel.Error;
                    break;
                default:
                    logLevel = LogEventLevel.Error;
                    break;
            }

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.RollingFile(new CompactJsonFormatter(), logFileOutPath)
                .MinimumLevel.Is(logLevel)
                .CreateLogger();
        }

        public static IServiceProvider ConfigureServices(){
            var fileWatcherConfiguration = _configuration.GetSection("FileWatcherSettings").Get<FileWatcherSettings>();
            var lotConfiguration = _configuration.GetSection("LotSettings").Get<LotSettings>();


            var serviceProvider = new ServiceCollection()
                .AddScoped<ILotAnalyzerService, LotAnalyzerService>()
                .AddScoped<IFileWatcher, FileWatcher>()
                .AddScoped<IStreamReader, Infrastructure.StreamReader.StreamReader>()
                .AddScoped<IReportGeneratorService, ReportGeneratorService>()
                .AddSingleton<FileWatcherSettings>(fileWatcherConfiguration)
                .AddSingleton<LotSettings>(lotConfiguration)
                .AddSingleton<ILogger>(_logger)
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
