using System.IO;
using SalesProcessor.Infrastructure.Configuration.FileWatcher;
using Serilog;

namespace SalesProcessor.Infrastructure.FileWatcher
{
    public class FileWatcher : IFileWatcher
    {
        private readonly FileSystemWatcher _watcher;
        private static ILogger _logger;
        private static FileWatcherSettings _configuration;

        public FileWatcher(ILogger logger, FileWatcherSettings configuration){
            _configuration = configuration;
            _logger = logger;
            _watcher = new FileSystemWatcher();
        }

        public void watch()
        {            
            //Ensure that Input folder exists to avoid errors in the watcher
            var inputFilesDirectory = _configuration.homePath + '\\' + _configuration.inDirectory;
            System.IO.Directory.CreateDirectory(inputFilesDirectory);

            _watcher.Path = inputFilesDirectory;
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _watcher.Filter = "*." + _configuration.inFileExtension;
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.EnableRaisingEvents = true;
            _logger.Information("Currently looking for new lots in the " + inputFilesDirectory + " folder!");
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            _logger.Information("A new Lot has been found and will be processed!");
        }
    }
}