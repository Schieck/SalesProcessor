namespace SalesProcessor.Test.UnitTest.Infrastructure
{
    using Xunit;
    using SalesProcessor.Infrastructure.Configuration.FileWatcher;
    using SalesProcessor.Infrastructure.Configuration.Logging;
    using SalesProcessor.Infrastructure.Configuration.FileGenerator;
    using SalesProcessor.Infrastructure.Configuration;
    using FluentAssertions;
    using SalesProcessor.Infrastructure.StreamReader;
    using System.IO;
    using SalesProcessor.Infrastructure.FileGenerator;
    using Moq;
    using Serilog;

    public class FileGenerator
    {
        IFileGenerator _sut;
        Mock<ILogger> _logger;
        FileGeneratorSettings _configuration;
        
        public FileGenerator(){
            _configuration = new FileGeneratorSettings();
            _logger = new Mock<ILogger>();
            _sut = new SalesProcessor.Infrastructure.FileGenerator.FileGenerator(_configuration, _logger.Object);
        }

        [Fact]
        public void GenerateFile_WhenCorrectFile_ShouldPass(){            
            //Arrange
            _configuration.outFileNamePattern = "{file_name}";
            _configuration.outFileExtension = ".done.dat";
            _configuration.outDirectory = "test/out";

            //Act
            _sut.GenerateFile("testing", "2a1sd654a84;");
        }
    }
}