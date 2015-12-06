using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapNShare.WindowsUtils;
using TestingInfrastructure.STA;
using Xunit;

namespace SnapNShare.Tests
{
    public class HotKeyHookTests
    {
        [STAFact]
        public void CanHookupOnKeyCombo()
        {
            using (var window = new MainWindow())
            {
                var isInvoked = false;
                using (var sut = new HotkeyHook((uint)KeyCodes.F2, (uint)KeyCodes.LCTRL, window, () => isInvoked = true))
                {
                    System.Windows.Forms.SendKeys.SendWait("^{F2}");
                }

                Assert.True(isInvoked);
            }
        }
    }
}
