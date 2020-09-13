namespace SalesProcessor.Test.UnitTest.Domain
{
    using System.IO;
    using System.Text;
    using Moq;
    using SalesProcessor.Domain.LotAnalyzer;
    using SalesProcessor.Infrastructure.StreamReader;
    using Serilog;
    using Serilog.Sinks.TestCorrelator;
    using Xunit;

    public class LotAnalyzerService
    {
        Mock<ILogger> _logger;
        Mock<SalesProcessor.Infrastructure.StreamReader.IStreamReader> _streamReader;
        ILotAnalyzerService _sut;

        public LotAnalyzerService(){
            _logger = new Mock<ILogger>();
            _streamReader = new Mock<SalesProcessor.Infrastructure.StreamReader.IStreamReader>();
            _sut = new SalesProcessor.Domain.LotAnalyzer.LotAnalyzerService(_logger.Object, _streamReader.Object);
        }

        [Fact]
        public void AnalyzeFileWithRetry_WhenCorrectFile_ShouldReadLine(){
            //Arrange
            string anyPath = "anyPath";
            FileInfo fileInfo = new FileInfo(anyPath + "anyfilename.something");
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("anyContent"));
            _streamReader.Setup(x => x.GetStreamReader(fileInfo.FullName)).Returns(new System.IO.StreamReader(ms));

            //Act
            _sut.AnalyzeFileWithRetry(fileInfo);

            //Assert
            _logger.Verify(l => l.Information("anyContent"));
            _streamReader.Verify(r => r.GetStreamReader(fileInfo.FullName), Times.Once);
        }

        [Fact]
        public void AnalyzeFileWithRetry_WhenCorrectFile_ShouldThrowError(){
            //Arrange
            string anyPath = "anyPath";
            FileInfo fileInfo = new FileInfo(anyPath + "anyfilename.something");
            _streamReader.Setup(x => x.GetStreamReader(fileInfo.FullName)).Throws(new IOException("Exception!"));

            //Act
            _sut.AnalyzeFileWithRetry(fileInfo);

            //Assert
            _logger.Verify(l => l.Warning(It.IsAny<string>()));
        }

        [Fact]
        public void AnalyzeDirectory_WhenCorrec_ShouldLogInformation(){
            //Arrange
            DirectoryInfo directoryInfo = new DirectoryInfo("./");

            //Act
            _sut.AnalyzeDirectory(directoryInfo, "*");

            //Assert
            _logger.Verify(l => l.Information(It.IsAny<string>()));
        }
    }
}