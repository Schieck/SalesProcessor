using System.IO;
using SalesProcessor.Infrastructure.Configuration.FileWatcher;
using SalesProcessor.Domain.LotAnalyzer;
using Serilog;

namespace SalesProcessor.Domain.FileWatcher
{
    public class FileWatcher : IFileWatcher
    {
        private readonly FileSystemWatcher _watcher;
        private static ILogger _logger;
        private static ILotAnalyzerService _lotAnalyzer;
        private static FileWatcherSettings _configuration;

        public FileWatcher(ILogger logger, ILotAnalyzerService lotAnalyzer, FileWatcherSettings configuration){
            _configuration = configuration;
            _logger = logger;
            _lotAnalyzer = lotAnalyzer;
            _watcher = new FileSystemWatcher();
        }

        public async void Watch()
        {            
            //Ensure that Input folder exists to avoid errors in the watcher
            var inputFilesDirectory = _configuration.homePath + '\\' + _configuration.inDirectory;
            System.IO.Directory.CreateDirectory(inputFilesDirectory);

            DirectoryInfo inputDirectory = new DirectoryInfo(inputFilesDirectory);

            _watcher.Path = inputFilesDirectory;
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _watcher.Filter = "*." + _configuration.inFileExtension;
            _watcher.Created += new FileSystemEventHandler(OnChanged);
            _watcher.EnableRaisingEvents = true;

            //In the program start we need to process all input directory, and after that if some lot is included the watcher will catch
            await _lotAnalyzer.AnalyzeDirectory(inputDirectory, _configuration.inFileExtension);

            _logger.Information("Currently looking for new lots in the " + inputFilesDirectory + " folder!");
        }

        public async void OnChanged(object source, FileSystemEventArgs e)
        {
            var fileInfo = new FileInfo(e.FullPath);
            await _lotAnalyzer.AnalyzeFileWithRetry(fileInfo);
        }        
    }
}