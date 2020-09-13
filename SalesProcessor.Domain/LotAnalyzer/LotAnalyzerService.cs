using System.IO;
using System.Threading.Tasks;
using SalesProcessor.Domain.ReportGenerator;
using SalesProcessor.Infrastructure.Configuration.Lot;
using SalesProcessor.Infrastructure.StreamReader;
using Serilog;

namespace SalesProcessor.Domain.LotAnalyzer
{
    public class LotAnalyzerService : ILotAnalyzerService
    {
        private static ILogger _logger;
        private static IStreamReader _streamReader;
        private static IReportGeneratorService _reportGeneratorService;
        private static LotSettings _lotconfiguration;

        public LotAnalyzerService(ILogger logger, IStreamReader streamReader, IReportGeneratorService reportGeneratorService, LotSettings lotconfiguration){
            _logger = logger;
            _streamReader = streamReader;
            _lotconfiguration = lotconfiguration;
            _reportGeneratorService = reportGeneratorService;
        }
        public async Task AnalyzeFileWithRetry(FileInfo file)
        {
            try{
               _logger.Information("Analyzing lot " + file.Name + "!");
               await AnalyzeFile(file);
            }catch{
                _logger.Warning("The lot " + file.Name + " failed analysis, the analysis will be retried!");
                
                if(AnalyzeFile(file).Exception != null){
                    _logger.Warning("The lot " + file.Name + " failed analysis in the retry, please validate this file and try again!");
                }                
            }
        }

        public async Task AnalyzeFile(FileInfo file){
            try{
                var lot = new Lot.Lot(_lotconfiguration);

                using (System.IO.StreamReader sr = _streamReader.GetStreamReader(file.FullName))
                {
                    string s;
                    int currentLine = 0;
                    while ((s = sr.ReadLine()) != null)
                    {
                        //We need to abstract this information to handle better and log if some information is not correct.
                        try{
                            lot.AddData(s);
                        }catch(System.Exception e){
                            _logger.Warning("Problem in line " + currentLine + " in the lot " + file.Name + " : " + e.Message);
                        }

                        currentLine++;
                    }   
                }

                //await _reportGeneratorService.GenerateReport(lot);
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

            _logger.Information("Completed files analysis in " + directory + " folder!");
        }
    }
}