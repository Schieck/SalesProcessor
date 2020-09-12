namespace SalesProcessor.Infrastructure.Configuration.FileGenerator
{
    public class FileGeneratorSettings : GenericSettings
    {
        public string outDirectory {get; set;}
        public string outFileNamePattern {get; set;} 
        public string outFileExtension {get; set;}
    }
}