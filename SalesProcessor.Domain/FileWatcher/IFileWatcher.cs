using System.IO;

namespace SalesProcessor.Domain.FileWatcher
{
    public interface IFileWatcher
    {
         void Watch();
         void OnChanged(object source, FileSystemEventArgs e);
    }
}