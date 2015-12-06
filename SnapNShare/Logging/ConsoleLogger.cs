using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.Logging
{
    public class ConsoleLogger : LoggerBase, ILogger
    {
        public Task LogException(Exception ex, string message)
        {
            Console.WriteLine(GetLogMessage(ex, message));

            return Task.FromResult<object>(null);
        }
    }
}
