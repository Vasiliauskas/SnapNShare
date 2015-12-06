using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace SnapNShare.WindowsUtils
{
    public class HotkeyHook : IDisposable
    {
        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey([In] IntPtr hWnd, [In] int id, [In] uint fsModifiers, [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey([In] IntPtr hWnd, [In] int id);

        private HwndSource _source;
        private readonly uint _hotkey;
        private readonly uint _modifier;
        private readonly Action _actionToInvoke;
        private readonly IntPtr _handle; 

        public HotkeyHook(uint hotKeyId, uint modifier, Window window, Action actionToInvoke)
        {
            _hotkey = hotKeyId;
            _modifier = modifier;
            _handle = new WindowInteropHelper(window).Handle;
            _source = HwndSource.FromHwnd(_handle);
            _source.AddHook(OnKeyPress);
            _actionToInvoke = actionToInvoke;
            if (!RegisterHotKey(_handle, GetType().GetHashCode(), _modifier, _hotkey))
                throw new InvalidOperationException(string.Format("Cannot register keys: {0} + {1}", _hotkey, _modifier));
        }

        private IntPtr OnKeyPress(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg.Equals(WM_HOTKEY))
                if (wParam.ToInt32().Equals(GetType().GetHashCode()))
                {
                    _actionToInvoke();
                    handled = true;
                }

            return IntPtr.Zero;
        }

        public void Dispose()
        {
            _source.RemoveHook(OnKeyPress);
            _source = null;
            UnregisterHotKey(_handle, GetType().GetHashCode());
        }
    }
}
