using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SnapNShare.ImageOutput;
using SnapNShare.ScreenCapture;
using SnapNShare.WindowsUtils;

namespace SnapNShare.Views
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window, IDisposable
    {
        private double _initialX;
        private double _initialY;
        private double _width;
        private double _height;
        private bool _isMouseDown = false;
        private ClippingRectangle _clippingRectangle;
        private System.Drawing.Bitmap _bitmap = null;
        private readonly IScreenCapture _screenCapture;
        private Point _initialWin32Point;
        private Point _currentWin32Point;

        public OverlayWindow(IScreenCapture screenCapture)
        {
            InitializeComponent();
            Stretch();
            FocusOnLoad();
            _screenCapture = screenCapture;
        }

        public System.Drawing.Bitmap GetClippedImage()
        {
            return _bitmap;
        }

        private void FocusOnLoad()
        {
            _cnv.Focus();
            Keyboard.Focus(_cnv);
        }

        private void Stretch()
        {
            Width = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;
        }

        #region Interaction events

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _txtBlock.Visibility = System.Windows.Visibility.Collapsed;
            _isMouseDown = true;
            var pos = GetMousePositionWPF();
            _initialWin32Point = GetMousePositionWin32();
            _initialX = pos.X;
            _initialY = pos.Y;
            if (_initialX <= 6.4)
                _initialX = 0;
            if (_initialY <= 6.4)
                _initialY = 0;
            e.Handled = true;
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this._isMouseDown)
            {
                // use relative coordinates for drawing
                var initialPosition = new Point(_initialX, _initialY);
                var currentPosition = GetMousePositionWPF();
                _currentWin32Point = GetMousePositionWin32();

                SwapPointsIfRequired(ref initialPosition, ref currentPosition);
                //SwapPointsIfRequired(ref _initialWin32Point, ref _currentWin32Point);

                _clippingRectangle.IfNull(CreateClippingRectangle);
                this.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
                this.Opacity = 1;
                _clippingRectangle.Arrange(initialPosition, currentPosition, _initialWin32Point, _currentWin32Point);
            }

            e.Handled = true;
        }

        private void SwapPointsIfRequired(ref Point initial, ref Point current)
        {
            double initialX = initial.X;
            double initialY = initial.Y;
            double currentX = current.X;
            double currentY = current.Y;

            if (initialX > currentX)
            {
                currentX = initial.X;
                initialX = current.X;
            }
            if (initialY > currentY)
            {
                currentY = initial.Y;
                initialY = current.Y;
            }

            initial = new Point(initialX, initialY);
            current = new Point(currentX, currentY);
        }

        private System.Windows.Point GetMousePositionWPF()
        {
            var currentPoint = Mouse.GetPosition(_cnv);
            return currentPoint;
        }

        private System.Windows.Point GetMousePositionWin32()
        {
            var currentPoint = MouseUtilsWin32.GetCursorPosition();
            return currentPoint;
        }

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            CaptureAndClose(e);

            e.Handled = true;
        }

        #endregion

        private void CreateClippingRectangle()
        {
            _cnv.Children.Clear();
            _clippingRectangle = new ClippingRectangle(_cnv);
        }

        private void CaptureAndClose(System.Windows.Input.MouseEventArgs e)
        {
            _clippingRectangle.IfNotNull(CleanupAndSave);

            _initialX = this._initialY = 0;
            _isMouseDown = false;
            Close();
        }

        private void CleanupAndSave()
        {
            _clippingRectangle.Dispose();

            // switch to absolute coordinates
            var currentPos = GetMousePositionWin32();
            SwapPointsIfRequired(ref _initialWin32Point, ref currentPos);

            _initialX = _initialWin32Point.X;
            _initialY = _initialWin32Point.Y;

            _width = currentPos.X - _initialX;
            _height = currentPos.Y - _initialY;

            if (_initialY >= 0 && _initialX >= 0 && _width > 0 && _height > 0)
                SaveScreen((int)_initialX, (int)_initialY, (int)_width, (int)_height);
        }

        private void SaveScreen(int x, int y, int width, int height)
        {
            var size = new System.Drawing.Size((int)width, (int)height);
            var point = new System.Drawing.Point((int)x, (int)y);

            _bitmap = _screenCapture.GetScreen(size, point);
        }

        public void Dispose()
        {
            Close();
        }
    }
}
