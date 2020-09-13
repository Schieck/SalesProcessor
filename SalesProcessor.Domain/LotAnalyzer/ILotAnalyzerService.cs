using System.IO;
using System.Threading.Tasks;

namespace SalesProcessor.Domain.LotAnalyzer
{
    public interface ILotAnalyzerService
    {
        Task AnalyzeFile(FileInfo file);
        Task AnalyzeFileWithRetry(FileInfo file);
        Task AnalyzeDirectory(DirectoryInfo directory, string inFileExtension);
    }
}