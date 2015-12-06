using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnapNShare;

namespace SnapNShare.ImageOutput
{
    public class ClipboardTarget : IOutputTarget
    {
        public Task SaveImage(System.Drawing.Bitmap bitmap)
        {
            Clipboard.SetImage(bitmap);

            return Task.FromResult<object>(null);
        }
    }
}
