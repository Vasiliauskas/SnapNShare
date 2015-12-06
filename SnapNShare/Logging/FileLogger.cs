using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.Logging
{
    public class FileLogger : LoggerBase, ILogger
    {
        private string _filePath;
        private string _workingDirectory;

        public FileLogger()
        {
            _workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (!Directory.Exists(_workingDirectory))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            _filePath = Path.Combine(_workingDirectory, "errors.log");
        }

        public async Task LogException(Exception ex, string message)
        {
            var log = GetLogMessage(ex, message);
            await WriteToFile(log);
        }

        public  string GetFilePath()
        {
            return _filePath;
        }

        private async Task WriteToFile(string message)
        {
            using (var writer = File.AppendText(_filePath))
                await writer.WriteLineAsync(message);
        }
    }
}
