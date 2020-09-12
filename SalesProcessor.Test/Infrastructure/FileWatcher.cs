namespace SalesProcessor.Tests.Infrastructure
{
    using Xunit;
    using SalesProcessor.Infrastructure.FileWatcher;
    using SalesProcessor.Infrastructure.Configuration.FileWatcher;
    using Serilog;
    using Serilog.Sinks.TestCorrelator;
    using FluentAssertions;
    using System.IO;

    public class FileWatcher
    {
        IFileWatcher _sut;
        FileWatcherSettings _configuration;
        ILogger _logger;
        public FileWatcher(){
            _logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).CreateLogger();;
            _configuration = new FileWatcherSettings(){
                inDirectory = "data/in",
                inFileExtension = "dat"
            };
            _sut = new SalesProcessor.Infrastructure.FileWatcher.FileWatcher(_logger, _configuration);
        }

        [Fact]
        public void Watch_WhenAlways_ShouldLogLooking(){
            //Arrange
            var inputFilesDirectory = _configuration.homePath + '\\' + _configuration.inDirectory;
            var expectedMessage = "Currently looking for new lots in the " + inputFilesDirectory + " folder!";
                        
            using (TestCorrelator.CreateContext())
            {
                //Act
                _sut.watch();

                //Assert
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be(expectedMessage);
            }
        }

        [Fact]
        public void OnChanged_WhenAlways_ShouldLog(){
            //Arrange
            object source = new object();
            FileSystemEventArgs e = null;
            var expectedMessage = "A new Lot has been found and will be processed!";

            using (TestCorrelator.CreateContext())
            {
                //Act
                _sut.OnChanged(source, e);

                //Assert
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be(expectedMessage);
            }
        }
    }
}