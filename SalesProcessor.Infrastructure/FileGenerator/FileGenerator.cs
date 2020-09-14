using System.IO;
using System.Text;
using System.Threading.Tasks;
using SalesProcessor.Infrastructure.Configuration.FileGenerator;
using Serilog;

namespace SalesProcessor.Infrastructure.FileGenerator
{
    public class FileGenerator : IFileGenerator
    {

        FileGeneratorSettings _configuration;
        ILogger _logger;

        public FileGenerator(FileGeneratorSettings configuration, ILogger logger){
            _configuration = configuration;
            _logger = logger;
        }

        public async Task GenerateFile(string filename, string content)
        {
            var finalFilename = _configuration.outFileNamePattern.Replace("file_name", filename);
            finalFilename = finalFilename + '.' + _configuration.outFileExtension;

            var outputFilesDirectory = _configuration.homePath + '\\' + _configuration.outDirectory + '\\';
            System.IO.Directory.CreateDirectory(outputFilesDirectory);

            var fullPath = outputFilesDirectory + finalFilename;
            
            try    
            {    
                // We`ll always override older files
                if (File.Exists(fullPath))    
                {    
                    File.Delete(fullPath);    
                }    
                
                // Create a new file     
                using (FileStream fs = File.Create(fullPath))     
                {    
                    // Add some text to file    
                    byte[] byteContent = new UTF8Encoding(true).GetBytes(content);    
                    await fs.WriteAsync(byteContent, 0, byteContent.Length);
                }

                _logger.Information("Report generated for lot " + filename + "!");
            }    
            catch (System.Exception Ex)
            {    
               _logger.Warning("Was not possible to generate the file. Error:" + Ex.Message);
            }
        }
    }
}