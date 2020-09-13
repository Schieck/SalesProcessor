using System.IO;

namespace SalesProcessor.Infrastructure.StreamReader
{
    public interface IStreamReader
    {
         System.IO.StreamReader GetStreamReader(string path);
    }
}