using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TestingInfrastructure.STA;
using Xunit;
using SnapNShare.Overlay;

namespace SnapNShare.Tests
{
    public class ClippingRectangleTests
    {
        [STAFact]
        public void ClippingRectangleCanBeInitialized()
        {
            var canvas = new Canvas();
            using (var sut = new ClippingRectangle(canvas))
                Assert.True(canvas.Children.Count > 0);
        }

    }
}
