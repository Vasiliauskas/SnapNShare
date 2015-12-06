using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SnapNShare.ImageOutput;

namespace SnapNShare.Views
{
    /// <summary>
    /// Interaction logic for TargetPicker.xaml
    /// </summary>
    public partial class TargetPicker : Window
    {
        private System.Drawing.Bitmap _bitmap;
        public TargetPicker(System.Drawing.Bitmap bitmap)
        {
            InitializeComponent();
            _bitmap = bitmap;
            var img = new BitmapImage();
            var stream = new MemoryStream();
            _bitmap.Save(stream, ImageFormat.Png);
            stream.Position = 0;
            img.BeginInit();
            img.StreamSource = stream;
            img.EndInit();
            this.DataContext = img;
        }

        private void ClipboardClick(object sender, RoutedEventArgs e)
        {
            using (_bitmap)
                new ClipboardTarget().SaveImage(_bitmap);
            this.Close();
        }

        private void FileClick(object sender, RoutedEventArgs e)
        {
            using (_bitmap)
                new FileTarget(new ImagePathProvider(System.Drawing.Imaging.ImageFormat.Png)).SaveImage(_bitmap);
            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
