namespace SalesProcessor.Test.UnitTest.Domain
{
    using System.IO;
    using System.Text;
    using Moq;
    using SalesProcessor.Domain.Customer;
    using SalesProcessor.Domain.Lot;
    using SalesProcessor.Domain.LotAnalyzer;
    using SalesProcessor.Domain.ReportGenerator;
    using SalesProcessor.Infrastructure.Configuration.Lot;
    using SalesProcessor.Infrastructure.StreamReader;
    using Serilog;
    using Serilog.Sinks.TestCorrelator;
    using Xunit;

    public class LotAnalyzerService
    {
        Mock<ILogger> _logger;
        Mock<SalesProcessor.Infrastructure.StreamReader.IStreamReader> _streamReader;
        Mock<LotSettings> _lotsettings;
        ILotAnalyzerService _sut;
        Mock<IReportGeneratorService> _reportGeneratorService;

        public LotAnalyzerService(){
            _logger = new Mock<ILogger>();
            _streamReader = new Mock<SalesProcessor.Infrastructure.StreamReader.IStreamReader>();
            _lotsettings = new Mock<LotSettings>();
            _reportGeneratorService = new Mock<IReportGeneratorService>();
            _sut = new SalesProcessor.Domain.LotAnalyzer.LotAnalyzerService(_logger.Object, _streamReader.Object, _reportGeneratorService.Object, _lotsettings.Object);
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
            _logger.Verify(l => l.Information("Analyzing lot " + fileInfo.Name + "!"));
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

        [Fact]
        public void AnalyzeFile_WhenCorrectLot_ShouldCallReportGenerator(){
             //Arrange
            string anyPath = "anyPath";
            FileInfo fileInfo = new FileInfo(anyPath + "anyfilename.something");
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("002ç2345675434544345çJose da SilvaçRural"));
            _streamReader.Setup(x => x.GetStreamReader(fileInfo.FullName)).Returns(new System.IO.StreamReader(ms));
            _lotsettings.Object.recordSeparator = "ç";
            var expectedCustomer = new SalesProcessor.Domain.Customer.Customer(){
                CNPJ = "2345675434544345"
            };

            //Act
            _sut.AnalyzeFile(fileInfo);

            //Assert
            _reportGeneratorService.Verify(x => x.GenerateReport(It.Is<SalesProcessor.Domain.Lot.Lot>(l => l.customers.Find(x => x.CNPJ == expectedCustomer.CNPJ).CNPJ == expectedCustomer.CNPJ)));
        }

         [Fact]
        public void AnalyzeFile_WhenWrongLot_ShoulLogLine(){
             //Arrange
            string anyPath = "anyPath";
            FileInfo fileInfo = new FileInfo(anyPath + "anyfilename.something");
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("007ç2345675434544345çJose da SilvaçRural"));
            _streamReader.Setup(x => x.GetStreamReader(fileInfo.FullName)).Returns(new System.IO.StreamReader(ms));
            _lotsettings.Object.recordSeparator = "ç";
            var expectedCustomer = new SalesProcessor.Domain.Customer.Customer(){
                CNPJ = "2345675434544345"
            };

            //Act
            Assert.ThrowsAsync<System.Exception>(() => _sut.AnalyzeFile(fileInfo));
            _logger.Verify(x => x.Warning(It.IsAny<string>()));            
        }
    }
}