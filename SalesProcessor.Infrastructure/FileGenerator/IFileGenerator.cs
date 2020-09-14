using System.Threading.Tasks;

namespace SalesProcessor.Infrastructure.FileGenerator
{
    public interface IFileGenerator
    {
        Task GenerateFile(string filename, string content);
    }
}