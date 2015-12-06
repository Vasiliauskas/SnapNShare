using TestingInfrastructure.STA;
using System.Windows;
using Xunit;
using SnapNShare.Overlay;
using System;

namespace SnapNShare.IntegrationTests
{
    public class OverlayTests
    {
        [STAFact]
        public void OverlayWindowStretchesOnFullDesktop()
        {
            using (var sut = new OverlayWindow(null))
            {
                sut.Show();
                Assert.True(sut.Width == SystemParameters.VirtualScreenWidth);
                Assert.True(sut.Height == SystemParameters.VirtualScreenHeight);
                sut.Close();
            }
        }

        [STAFact]
        public void OverlayWindowIsCloseByEngine()
        {
            var sut = new OverlayEngine(null);
            var overlayWindow = new OverlayWindow(sut);
            overlayWindow.Show();
            sut.Dispose();

            Assert.False(overlayWindow.IsVisible);
        }

        [STAFact]
        public void OverlayWindowImageIsNullWithoutCapture()
        {
            using (var sut = new OverlayEngine(null))
            using (var overlayWindow = new OverlayWindow(sut))
            {
                overlayWindow.Show();
                var result = sut.GetClippedImage();
                Assert.Null(result);
            }
        }
    }
}
