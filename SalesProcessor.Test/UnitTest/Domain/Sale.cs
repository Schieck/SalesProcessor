namespace SalesProcessor.Test.UnitTest.Domain
{
    using Xunit;
    using SalesProcessor.Domain.Sale;
    using System.Collections.Generic;

    public class Sale
    {
        [Fact]
        public void Sale_WhenAlways_HaveProperties(){
            SalesProcessor.Domain.Sale.Sale test = new SalesProcessor.Domain.Sale.Sale();
            test.id = "test";
            test.salesManName = "test";
            test.items = new List<SalesProcessor.Domain.Sale.Item.Item>();
        }

        [Fact]
        public void Item_WhenAlways_HaveProperties(){
            SalesProcessor.Domain.Sale.Item.Item test = new SalesProcessor.Domain.Sale.Item.Item();
            test.id = "test";
            test.price = 0.05M;
            test.quantity = 25;
        }

        [Fact]
        public void Generate_WhenCorrectData_ShouldCreateSale(){
            //Arrange
            string[] sale = {"003", "10", "[1-10-100,2-30-2.50,3-40-3.10]", "Pedro"};

            //Act
            var result = SalesProcessor.Domain.Sale.GenerateByData.Generate(sale);

            //Assert
            Assert.Equal(sale[1], result.id);
            Assert.NotEmpty(result.items);
            Assert.Equal(sale[3], result.salesManName);
        }

        [Fact]
        public void Generate_WhenIncorrectData_ShouldReturnNull(){
            //Arrange
            string[] sale = {};

            //Act
            Assert.Null(SalesProcessor.Domain.Sale.GenerateByData.Generate(sale));
        }
    }
}