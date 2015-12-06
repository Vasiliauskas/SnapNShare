using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.ScreenCapture
{
    public interface IScreenCapture
    {
        Bitmap GetScreen(Size size, Point startingPoint);
    }
}
