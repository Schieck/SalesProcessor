namespace SalesProcessor.Test.UnitTest.Domain
{
    using Xunit;
    using SalesProcessor.Domain.FileWatcher;
    using SalesProcessor.Infrastructure.Configuration.FileWatcher;
    using Serilog;
    using Serilog.Sinks.TestCorrelator;
    using FluentAssertions;
    using System.IO;
    using Moq;
    using SalesProcessor.Domain.LotAnalyzer;
    using System.Threading.Tasks;

    public class FileWatcher
    {
        IFileWatcher _sut;
        FileWatcherSettings _configuration;
        Mock<ILotAnalyzerService> _lotAnalyzerService;
        ILogger _logger;
        public FileWatcher(){
            _logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).CreateLogger();
            _configuration = new FileWatcherSettings(){
                inDirectory = "data/in",
                inFileExtension = "dat"
            };
            _lotAnalyzerService = new Mock<ILotAnalyzerService>();
            _sut = new SalesProcessor.Domain.FileWatcher.FileWatcher(_logger, _lotAnalyzerService.Object, _configuration);
        }

        [Fact]
        public void Watch_WhenAlways_ShouldLogLooking(){
            //Arrange
            var inputFilesDirectory = _configuration.homePath + '\\' + _configuration.inDirectory;
            var expectedMessage = "Currently looking for new lots in the " + inputFilesDirectory + " folder!";
                        
            using (TestCorrelator.CreateContext())
            {
                //Act
                _sut.Watch();

                //Assert
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be(expectedMessage);
            }
        }

        [Fact]
        public void OnChanged_WhenAlways_ShouldCallAnalyzeFileWithRetry(){
            //Arrange
            object source = new object();
            FileSystemEventArgs e = new FileSystemEventArgs(WatcherChangeTypes.Created, "", "Test");
            var expected = new FileInfo(e.FullPath);
            
            //Act
            _sut.OnChanged(source, e);

            //Assert
            _lotAnalyzerService.Verify(m => m.AnalyzeFileWithRetry(It.Is<FileInfo>(x => x.Name == "Test")));
        }
    }
}