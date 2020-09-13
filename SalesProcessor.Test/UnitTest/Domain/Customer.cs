namespace SalesProcessor.Test.UnitTest.Domain
{
    using Xunit;
    using SalesProcessor.Domain.Customer;

    public class Customer
    {
        [Fact]
        public void Customer_WhenAlways_HaveProperties(){
            SalesProcessor.Domain.Customer.Customer test = new SalesProcessor.Domain.Customer.Customer();
            test.CNPJ = "test";
            test.BusinessArea = "test";
            test.Name = "test";
        }

        [Fact]
        public void Generate_WhenCorrectData_ShouldCreateCustomer(){
            //Arrange
            string[] customer = {"002","2345675434544345", "Jose da Silva", "Rural"};

            //Act
            var result = SalesProcessor.Domain.Customer.GenerateByData.Generate(customer);

            //Assert
            Assert.Equal(customer[1], result.CNPJ);
            Assert.Equal(customer[2], result.Name);
            Assert.Equal(customer[3], result.BusinessArea);
        }

        [Fact]
        public void Generate_WhenIncorrectData_ShouldReturnNull(){
            //Arrange
            string[] customer = {};

            //Act
            Assert.Null(SalesProcessor.Domain.Customer.GenerateByData.Generate(customer));
        }
    }
}