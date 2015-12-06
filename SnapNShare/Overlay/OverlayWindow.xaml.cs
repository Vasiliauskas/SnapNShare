using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SnapNShare.Overlay
{
    public partial class OverlayWindow : Window, IDisposable
    {
        private readonly OverlayEngine _overlayEngine;
        public OverlayWindow(OverlayEngine overLayEngine)
        {
            InitializeComponent();
            Stretch();
            FocusOnLoad();
            _overlayEngine = overLayEngine;
            _overlayEngine.IfNotNull(() => _overlayEngine.Init(this, _cnv));
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
            _overlayEngine.MouseDown();
            e.Handled = true;
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed || 
                e.RightButton == MouseButtonState.Pressed || 
                e.MiddleButton == MouseButtonState.Pressed)
            {
                _overlayEngine.MouseMove();
                Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
                Opacity = 1;
            }
            e.Handled = true;
        }

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _overlayEngine.MouseUp();
            e.Handled = true;
        }

        #endregion

        public void Dispose()
        {
            Close();
        }
    }
}
