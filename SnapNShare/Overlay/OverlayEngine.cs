using SnapNShare.ScreenCapture;
using SnapNShare.Views;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using SnapNShare.WindowsUtils;
using System;
using SnapNShare.Extensions;

namespace SnapNShare.Overlay
{
    public class OverlayEngine : IDisposable
    {
        // used for WPF dpi based zooming coordinates
        private double _initialX;
        private double _initialY;
        private double _width;
        private double _height;

        private bool _isMouseDown = false;

        private ClippingRectangle _clippingRectangle;
        private System.Drawing.Bitmap _bitmap = null;

        // used for actual pixel coordinates
        private Point _initialWin32Point;
        private Point _currentWin32Point;

        private Canvas _sourceCanvas;
        private Window _sourceWindow;
        private readonly IScreenCapture _screenCapture;

        public OverlayEngine(IScreenCapture screenCapture)
        {
            _screenCapture = screenCapture;
        }

        public void Init(Window window, Canvas canvas)
        {
            _sourceCanvas = canvas;
            _sourceWindow = window;
        }

        public System.Drawing.Bitmap GetClippedImage()
        {
            return _bitmap;
        }

        public void MouseDown()
        {
            _isMouseDown = true;
            var pos = GetMousePositionWPF();
            _initialWin32Point = GetMousePositionWin32();
            _initialX = pos.X;
            _initialY = pos.Y;
            if (_initialX <= 6.4)
                _initialX = 0;
            if (_initialY <= 6.4)
                _initialY = 0;
        }

        public void MouseMove()
        {
            if (_isMouseDown)
            {
                // use relative coordinates for drawing
                var initialPosition = new Point(_initialX, _initialY);
                var currentPosition = GetMousePositionWPF();
                _currentWin32Point = GetMousePositionWin32();

                Geometry.SwapPointsIfRequired(ref initialPosition, ref currentPosition);

                _clippingRectangle.IfNull(CreateClippingRectangle);
                _clippingRectangle.Arrange(initialPosition, currentPosition, _initialWin32Point, _currentWin32Point);
            }
        }

        public void MouseUp()
        {
            CaptureAndClose();
        }

        private System.Windows.Point GetMousePositionWPF()
        {
            var currentPoint = Mouse.GetPosition(_sourceCanvas);
            return currentPoint;
        }

        private System.Windows.Point GetMousePositionWin32()
        {
            var currentPoint = MouseUtilsWin32.GetCursorPosition();
            return currentPoint;
        }

        private void CreateClippingRectangle()
        {
            _sourceCanvas.Children.Clear();
            _clippingRectangle = new ClippingRectangle(_sourceCanvas);
        }

        private void CleanupAndSave()
        {
            _clippingRectangle.Dispose();

            // switch to absolute coordinates
            var currentPos = GetMousePositionWin32();
            Geometry.SwapPointsIfRequired(ref _initialWin32Point, ref currentPos);

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

        private void CaptureAndClose()
        {
            _clippingRectangle.IfNotNull(CleanupAndSave);
            _initialX = _initialY = 0;
            _isMouseDown = false;
            Dispose();
        }

        public void Dispose()
        {
            _sourceCanvas.Children.Clear();
            _sourceWindow.Close();
        }
    }
}
