using System.IO;

namespace SalesProcessor.Infrastructure.FileWatcher
{
    public interface IFileWatcher
    {
         void watch();
         void OnChanged(object source, FileSystemEventArgs e);
    }
}