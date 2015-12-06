using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.Logging
{
    public interface ILogger
    {
        Task LogException(Exception ex, string message);
    }
}
