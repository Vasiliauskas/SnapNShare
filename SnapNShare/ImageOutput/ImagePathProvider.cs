using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.ImageOutput
{
    public interface IPathProvider
    {
        string GetFilePath();
        ImageFormat GetFormat();
    }

    public class ImagePathProvider : IPathProvider
    {
        private string _extension;
        private string _filter;
        private ImageFormat _format;

        public ImagePathProvider(ImageFormat format)
        {
            _format = format;
            _extension = format.ToString();

            switch (_extension)
            {
                case "Png":
                    _filter = "Png Files|*.png";
                    break;
                default:
                    break;
            }
        }

        public ImageFormat GetFormat()
        {
            return _format;
        }

        public string GetFilePath()
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = _extension;
            saveFileDialog.Filter = _filter;
            saveFileDialog.FileName = "myScreenShot.png";
            var res = saveFileDialog.ShowDialog();
            if (res.HasValue && res.Value)
                return saveFileDialog.FileName;

            return string.Empty;
        }
    }
}
