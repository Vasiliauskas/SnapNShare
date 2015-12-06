using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapNShare.ScreenCapture;
using Xunit;

namespace SnapNShare.Tests
{
    public class Win32ScreenCaptureTests
    {
        [Theory]
        [InlineData(100, 100)]
        [InlineData(2000, 2000)]
        public void CanCaptureScreen(int width, int height)
        {
            var sut = new Win32ScreenCapture();
            var result = sut.GetScreen(new System.Drawing.Size(width, height), new System.Drawing.Point(0, 0));

            Assert.NotNull(result);
            Assert.True(result.Height == height && result.Width == width);
        }
    }
}
