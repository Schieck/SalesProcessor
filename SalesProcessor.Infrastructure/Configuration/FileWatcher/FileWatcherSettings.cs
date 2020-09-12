namespace SalesProcessor.Infrastructure.Configuration.FileWatcher
{
    public class FileWatcherSettings : GenericSettings
    {
        public string inDirectory {get; set;}
        public string inFileExtension {get; set;}
    }
}