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

    public class StreamReader
    {
        IStreamReader _sut;

        public StreamReader(){
            _sut = new SalesProcessor.Infrastructure.StreamReader.StreamReader();
        }

        [Fact]
        public void GetStreamReader_WhenCorrect_ShouldReturnNewReader(){
            
            //Act
            var result = _sut.GetStreamReader("./appsettings.json");

            //Assert
            Assert.Equal(result.GetType(), System.Type.GetType("System.IO.StreamReader"));
        }

        [Fact]
        public void GetStreamReader_WhenNotValidPath_ShouldReturnException(){
            //Act and Assert
            Assert.Throws<FileNotFoundException>(() => _sut.GetStreamReader("idonotexists"));
        }
    }
}