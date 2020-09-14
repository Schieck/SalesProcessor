namespace SalesProcessor.Test.UnitTest.Domain
{
    using Moq;
    using SalesProcessor.Domain.ReportGenerator;
    using SalesProcessor.Infrastructure.Configuration.Lot;
    using SalesProcessor.Infrastructure.FileGenerator;
    using Xunit;
    public class ReportGenerator
    {
        IReportGeneratorService _sut;

        Mock<IFileGenerator> _fileGenerator;

        public ReportGenerator(){
            _fileGenerator = new Mock<IFileGenerator>();
            _sut = new SalesProcessor.Domain.ReportGenerator.ReportGeneratorService(_fileGenerator.Object);
        }

        [Fact]
        public void GenerateReport_WhenCorrectLot_GenerateCorrectString(){
            
            //Arrange
            var lot = new SalesProcessor.Domain.Lot.Lot(new LotSettings(){recordSeparator = "ç"}, "Test");

            //Act
            _sut.GenerateReport(lot);

            //Assert
            _fileGenerator.Verify(x => x.GenerateFile(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}