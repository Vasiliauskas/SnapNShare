using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapNShare.WindowsUtils
{
    public static class StartupManager
    {
        public static readonly string APP_NAME = "SnapNShare";
        public static void EnsureStartup()
        {
            using (RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (rkApp.GetValue(APP_NAME) == null)
                    rkApp.SetValue(APP_NAME, System.Reflection.Assembly.GetEntryAssembly().Location);
            }
        }
    }
}
