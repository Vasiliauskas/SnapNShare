using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.Logging
{
    public class CompositeLogger : ILogger
    {
        private List<ILogger> _loggers;
        public CompositeLogger(params ILogger[] loggers)
        {
            _loggers = new List<ILogger>();
            foreach (var logger in loggers)
                _loggers.Add(logger);
        }
        public async Task LogException(Exception ex, string message)
        {
            foreach (var logger in _loggers)
                await logger.LogException(ex, message);
        }
    }
}
