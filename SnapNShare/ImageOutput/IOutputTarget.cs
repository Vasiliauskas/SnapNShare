using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.ImageOutput
{
    public interface IOutputTarget
    {
        Task SaveImage(Bitmap bitmap);
    }
}
