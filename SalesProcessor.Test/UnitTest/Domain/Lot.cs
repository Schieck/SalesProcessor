namespace SalesProcessor.Test.UnitTest.Domain
{
    using Xunit;
    using SalesProcessor.Domain.Lot;
    using Moq;
    using SalesProcessor.Infrastructure.Configuration.Lot;

    public class Lot
    {
        Mock<LotSettings> _configuration;

        public Lot(){     
            _configuration = new Mock<LotSettings>();       
            _configuration.Object.recordSeparator = "ç";
        }

        [Fact]
        public void AddData_WhenSales_ShouldAddSale(){
            //Arrange
            var line = "003ç10ç[1-10-100,2-30-2.50,3-40-3.10]çPedro ";
            var lot = new SalesProcessor.Domain.Lot.Lot(_configuration.Object);
            
            //Act
            lot.AddData(line);

            //Assert
            Assert.NotEmpty(lot.sales);
        }

        [Fact]
        public void AddData_WhenCustomer_ShouldAddCustomer(){
            //Arrange
            var line = "002ç2345675434544345çJose da SilvaçRural";
            var lot = new SalesProcessor.Domain.Lot.Lot(_configuration.Object);
            
            //Act
            lot.AddData(line);

            //Assert
            Assert.NotEmpty(lot.customers);
        }

        [Fact]
        public void AddData_WhenSalesperson_ShouldAddSalesperson(){
            //Arrange
            var line = "001ç1234567891234çPedroç50000";
            var lot = new SalesProcessor.Domain.Lot.Lot(_configuration.Object);
            
            //Act
            lot.AddData(line);

            //Assert
            Assert.NotEmpty(lot.salespersons);
        }

        [Fact]
        public void AddData_WhenNotRecognized_ShouldThrow(){
            //Arrange
            var line = "008ç2345675434544345çJose da SilvaçRural";
            var lot = new SalesProcessor.Domain.Lot.Lot(_configuration.Object);
            
            //Act
            Assert.ThrowsAny<System.Exception>(() => lot.AddData(line));
        }

        [Fact]
        public void AddSale_WhenWrongData_ShouldThrow(){

            //Arrange
            var lot = new SalesProcessor.Domain.Lot.Lot(_configuration.Object);

            //Act and Assert
            Assert.ThrowsAny<System.Exception>(() => lot.AddSale(null));
        }

        [Fact]
        public void AddSalesperson_WhenWrongData_ShouldThrow(){

            //Arrange
            var lot = new SalesProcessor.Domain.Lot.Lot(_configuration.Object);

            //Act and Assert
            Assert.ThrowsAny<System.Exception>(() => lot.AddSalesPerson(null));
        }

        [Fact]
        public void AddCustomer_WhenWrongData_ShouldThrow(){

            //Arrange
            var lot = new SalesProcessor.Domain.Lot.Lot(_configuration.Object);

            //Act and Assert
            Assert.ThrowsAny<System.Exception>(() => lot.AddCustomer(null));
        }

    }
}