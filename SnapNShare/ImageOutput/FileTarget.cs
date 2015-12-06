using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.ImageOutput
{
    public class FileTarget : IOutputTarget
    {
        private IPathProvider _pathProvider;

        public FileTarget(IPathProvider provider)
        {
            _pathProvider = provider;
        }

        public Task SaveImage(System.Drawing.Bitmap bitmap)
        {
            var path = _pathProvider.GetFilePath();
            if (!string.IsNullOrEmpty(path))
            {
                bitmap.Save(path, _pathProvider.GetFormat());
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", path));
            }

            return Task.FromResult<object>(null);
        }
    }
}
