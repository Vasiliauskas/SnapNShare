using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapNShare.ImageOutput;
using TestingInfrastructure.STA;
using Xunit;

namespace SnapNShare.Tests
{
    public class ImageOutputTests
    {
        private Bitmap _bitmap;
        public ImageOutputTests()
        {
            _bitmap = new Bitmap(10, 10);
        }

        [STAFact]
        public async Task CanOutputToClipboard()
        {
            var sut = new ClipboardTarget();
            await sut.SaveImage(_bitmap);

            var result = System.Windows.Forms.Clipboard.GetImage();

            Assert.NotNull(result);
        }

        [STAFact]
        public async Task CanOutputToFile()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "test.png");
            var mock = new Mock<IPathProvider>();
            mock.Setup(x => x.GetFilePath()).Returns(path);
            mock.Setup(x => x.GetFormat()).Returns(ImageFormat.Png);
            var sut = new FileTarget(mock.Object);

            await sut.SaveImage(_bitmap);

            Assert.True(File.Exists(path));

            File.Delete(path);
        }
    }
}
