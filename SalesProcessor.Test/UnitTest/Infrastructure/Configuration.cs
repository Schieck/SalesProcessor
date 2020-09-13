namespace SalesProcessor.Test.UnitTest.Infrastructure
{
    using Xunit;
    using SalesProcessor.Infrastructure.Configuration.FileWatcher;
    using SalesProcessor.Infrastructure.Configuration.Logging;
    using SalesProcessor.Infrastructure.Configuration.FileGenerator;
    using SalesProcessor.Infrastructure.Configuration;
    using FluentAssertions;

    public class Configuration
    {
        [Fact]
        public void FileWatcherSettings_WhenAlways_ShouldReceiveParameters(){
            //Arrange
            var fileWatcherSettings = new FileWatcherSettings();
            var expectedValue = "test";
            
            //Act
            fileWatcherSettings.inFileExtension = expectedValue;
            fileWatcherSettings.inDirectory = expectedValue;

            //Assert
            Assert.Equal(fileWatcherSettings.inDirectory, expectedValue);
            Assert.Equal(fileWatcherSettings.inFileExtension, expectedValue);
        }

        [Fact]
        public void FileWatcherSettings_WhenAlways_ShouldGetHomePath(){
            //Arrange
            var fileWatcherSettings = new FileWatcherSettings();
            var genericSettings = new GenericSettings();

            //Act
            var result = fileWatcherSettings.homePath;

            //Assert
            Assert.Equal(genericSettings.homePath, result);
        }

        [Fact]
        public void FileGeneratorSettings_WhenAlways_ShouldReceiveParameters(){
            //Arrange
            var fileGeneratorSettings = new FileGeneratorSettings();
            var expectedValue = "test";
            
            //Act
            fileGeneratorSettings.outDirectory = expectedValue;
            fileGeneratorSettings.outFileExtension = expectedValue;
            fileGeneratorSettings.outFileNamePattern = expectedValue;

            //Assert
            Assert.Equal(fileGeneratorSettings.outDirectory, expectedValue);
            Assert.Equal(fileGeneratorSettings.outFileExtension, expectedValue);
            Assert.Equal(fileGeneratorSettings.outFileNamePattern, expectedValue);
        }

        [Fact]
        public void FileGeneratorSettings_WhenAlways_ShouldGetHomePath(){
            //Arrange
            var fileGeneratorSettings = new FileGeneratorSettings();
            var genericSettings = new GenericSettings();

            //Act
            var result = fileGeneratorSettings.homePath;

            //Assert
            Assert.Equal(genericSettings.homePath, result);
        }

                [Fact]
        public void LoggingSettings_WhenAlways_ShouldReceiveParameters(){
            //Arrange
            var loggingSettings = new LoggingSettings();
            var expectedValue = "test";
            
            //Act
            loggingSettings.logFileOutput = expectedValue;
            loggingSettings.logLevel = expectedValue;

            //Assert
            Assert.Equal(loggingSettings.logFileOutput, expectedValue);
            Assert.Equal(loggingSettings.logLevel, expectedValue);
        }

        [Fact]
        public void LoggingSettings_WhenAlways_ShouldGetHomePath(){
            //Arrange
            var loggingSettings = new LoggingSettings();
            var genericSettings = new GenericSettings();

            //Act
            var result = loggingSettings.homePath;

            //Assert
            Assert.Equal(genericSettings.homePath, result);
        }
    }
}