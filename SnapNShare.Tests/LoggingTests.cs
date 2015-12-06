using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapNShare.Logging;
using TestingInfrastructure.AutoData;
using Xunit;

namespace SnapNShare.Tests
{
    public class LoggingTests
    {
        [Theory]
        [DefaultAutoData]
        public async Task CompositeLoggerCanLogMessage(string message)
        {
            string loggedMessage = string.Empty;
            Mock<ILogger> memoryLogger = new Mock<ILogger>();
            memoryLogger.Setup(x => x.LogException(null, message)).Returns(() =>
            {
                loggedMessage = message;
                return Task.FromResult<object>(null);
            });

            var sut = new CompositeLogger(memoryLogger.Object);
            await sut.LogException(null, message);

            Assert.Equal(message, loggedMessage);
        }

        [Theory]
        [DefaultAutoData]
        public async Task EmptyMessagesCannotBeLogged(ConsoleLogger sut)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.LogException(null, null));
        }

        [Theory]
        [DefaultAutoData]
        public async Task CanLogToAFile(FileLogger sut)
        {
            await sut.LogException(null, "test");
            var filePath = sut.GetFilePath();

            Assert.True(File.Exists(filePath));

            File.Delete(filePath);
        }
    }
}
