using System.IO;
using System.Threading.Tasks;
using SalesProcessor.Infrastructure.StreamReader;
using Serilog;

namespace SalesProcessor.Domain.LotAnalyzer
{
    public class LotAnalyzerService : ILotAnalyzerService
    {
        private static ILogger _logger;
        private static IStreamReader _streamReader;

        public LotAnalyzerService(ILogger logger, IStreamReader streamReader){
            _logger = logger;
            _streamReader = streamReader;
        }
        public async Task AnalyzeFileWithRetry(FileInfo file)
        {
            try{
                _logger.Information("Analyzing lot " + file.Name + "!");
               await AnalyzeFile(file);
            }catch(IOException e){
                _logger.Warning("The lot " + file.Name + " failed analysis, the analysis will be retried!");
                
                if(AnalyzeFile(file).Exception != null){
                    _logger.Warning("The lot " + file.Name + " failed analysis in the retry, please validate this file and try again!");
                }                
            }
        }

        public async Task AnalyzeFile(FileInfo file){
            try{
                using (System.IO.StreamReader sr = _streamReader.GetStreamReader(file.FullName))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        _logger.Information(s);
                    }                
                }
            }catch(IOException e){
                _logger.Warning( "Error ocurred when opening lot:" + e.Message);
                throw e;
            }

        }

        public async Task AnalyzeDirectory(DirectoryInfo directory, string inFileExtension){
            
            _logger.Information("Analyzing already existed files in " + directory.Name + " folder!");

            foreach (var file in directory.GetFiles("*." + inFileExtension))
            {
                await this.AnalyzeFileWithRetry(file);
            }

            _logger.Information("Completed files analisis in " + directory + " folder!");
        }
    }
}