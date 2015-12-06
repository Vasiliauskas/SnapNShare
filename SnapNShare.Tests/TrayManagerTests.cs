using System;
using System.Linq;
using SnapNShare.WindowsUtils;
using Xunit;
using Xunit.Sdk;

namespace SnapNShare.Tests
{
    public class TrayManagerTests
    {
        [Fact]
        public void CanShowTrayIcon()
        {
            using (var sut = new TrayManager(() => { }))
                Assert.True(sut.IsTrayVisible);
        }

        [Fact]
        public void TrayManagerThrowsOnNullArgument()
        {
            Assert.Throws(typeof(ArgumentNullException), () => new TrayManager(null));
        }

        [Fact]
        public void TrayManagerCanDispose()
        {
            var sut = new TrayManager(() => { });
            sut.Dispose();
            Assert.True(!sut.IsTrayVisible);
        }
    }
}
