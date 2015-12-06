using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapNShare.WindowsUtils
{
    public class TrayManager : IDisposable
    {
        private Action _actionToInvoke;
        private NotifyIcon _trayIcon;

        public bool IsTrayVisible { get { return _trayIcon.Visible; } }

        public TrayManager(Action actionToInvoke)
        {
            if (actionToInvoke == null)
                throw new ArgumentNullException("Action to invoke cannot be null");

            _actionToInvoke = actionToInvoke;
            CreateIcon();
        }

        public void Dispose()
        {
            _trayIcon.DoubleClick -= ExecuteAction;
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
        }

        private NotifyIcon CreateIcon()
        {
            _trayIcon = new NotifyIcon();
            _trayIcon.Icon = SnapNShare.Properties.Resources.tray;
            _trayIcon.Visible = true;
            _trayIcon.DoubleClick += ExecuteAction;
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem("Capture screen", (o, e) => ExecuteAction(null, EventArgs.Empty)));
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(new MenuItem("Exit", (o, e) => App.Current.Shutdown(1)));
            _trayIcon.ContextMenu = contextMenu;

            return _trayIcon;
        }

        private void ExecuteAction(object sender, EventArgs args)
        {
            _actionToInvoke();
        }
    }
}
