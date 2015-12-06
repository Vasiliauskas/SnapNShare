using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapNShare.WindowsUtils;
using Xunit;

namespace SnapNShare.Tests
{

    public class AutoStartupTests
    {
        [Fact(Skip="Bad design with assembly path")]
        public void AutoStartupCanBeAdded()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            Assert.True(rkApp.GetValue(StartupManager.APP_NAME) == null);

            StartupManager.EnsureStartup();

            rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            Assert.False(rkApp.GetValue(StartupManager.APP_NAME) == null);

            rkApp.DeleteValue(StartupManager.APP_NAME);
            rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            Assert.True(rkApp.GetValue(StartupManager.APP_NAME) == null);
        }

    }
}
