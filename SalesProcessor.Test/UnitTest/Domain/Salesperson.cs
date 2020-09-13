namespace SalesProcessor.Test.UnitTest.Domain
{
    using Xunit;
    using SalesProcessor.Domain.Salesperson;

    public class Salesperson
    {
        [Fact]
        public void Salesperson_WhenAlways_HaveProperties(){
            SalesProcessor.Domain.Salesperson.Salesperson test = new SalesProcessor.Domain.Salesperson.Salesperson();
            test.CPF = "test";
            test.Name = "test";
            test.Salary = 5000.52M;
        }

        [Fact]
        public void Generate_WhenCorrectData_ShouldCreateSalesPerson(){
            //Arrange
            string[] salesPerson = {"001","1234567891234","Pedro","50000"};

            //Act
            var result = SalesProcessor.Domain.Salesperson.GenerateByData.Generate(salesPerson);

            //Assert
            Assert.Equal(salesPerson[1], result.CPF);
            Assert.Equal(salesPerson[2], result.Name);
            Assert.Equal(decimal.Parse(salesPerson[3]), result.Salary);
        }

        [Fact]
        public void Generate_WhenIncorrectData_ShouldReturnNull(){
            //Arrange
            string[] salesPerson = {};

            //Act
            Assert.Null(SalesProcessor.Domain.Salesperson.GenerateByData.Generate(salesPerson));
        }
    }
}