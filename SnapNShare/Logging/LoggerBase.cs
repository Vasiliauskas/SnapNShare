using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.Logging
{
    public abstract class LoggerBase
    {
        protected virtual string GetLogMessage(Exception ex, string message)
        {
            if (ex == null && string.IsNullOrEmpty(message))
                throw new InvalidOperationException("Cannot build message, both parameters null");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("--------------Exception-----{0}----", DateTime.Now));
            sb.Append(Environment.NewLine);
            if (ex != null)
            {
                sb.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    sb.AppendLine(string.Format("-----INNER: {0}", ex.InnerException.Message));
                    sb.Append(Environment.NewLine);
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                sb.AppendLine(string.Format("-----MESSAGE: {0}", message));
                sb.Append(Environment.NewLine);
            }

            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
    }
}
