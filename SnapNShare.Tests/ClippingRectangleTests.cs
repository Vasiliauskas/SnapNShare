using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SnapNShare.Views;
using TestingInfrastructure.STA;
using Xunit;

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
