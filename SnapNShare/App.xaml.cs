using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SnapNShare.Logging;
using SnapNShare.ScreenCapture;
using SnapNShare.Views;
using SnapNShare.WindowsUtils;

namespace SnapNShare
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly TrayManager _trayManager;
        private readonly MainWindow _window;
        private static ILogger _logger;
        private HotkeyHook _winSHotkeyHook;
        private bool _isOverlayActive;
        private OverlayWindow _overlayWindow;
        public static ILogger Logger
        {
            get
            {
                if (_logger == null)
                    _logger = new CompositeLogger(new FileLogger(), new ConsoleLogger());
                return _logger;
            }
        }

        public App()
        {
            _window = new MainWindow();
            _window.Show();
            _trayManager = new TrayManager(ShowOverlayWindow);
            _winSHotkeyHook = new HotkeyHook((uint)KeyCodes.F2, (uint)KeyCodes.LCTRL, _window, ShowOverlayWindow);

            StartupManager.EnsureStartup();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogException(e.Exception, null);
        }

        private void ShowOverlayWindow()
        {
            if (_isOverlayActive)
            {
                _overlayWindow.Close();
                _isOverlayActive = false;
            }

            _isOverlayActive = true;
            _overlayWindow = new OverlayWindow(new Win32ScreenCapture());
            _overlayWindow.WindowState = WindowState.Maximized;
            _overlayWindow.ShowDialog();
            var img = _overlayWindow.GetImage();
            img.IfNotNull(ExportImage);

            _isOverlayActive = false;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _trayManager.Dispose();
            base.OnExit(e);
        }

        private void ExportImage(System.Drawing.Bitmap bitmap)
        {
            var picker = new TargetPicker(bitmap);
            picker.Owner = _window;
            picker.Topmost = true;
            picker.ShowDialog();
        }
    }
}
