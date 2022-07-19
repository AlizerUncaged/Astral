using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class ForegroundWindow : IUtility
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Models.Win32.Rect lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        public Models.Win32.Rect GetForegroundWindowBounds()
        {
            var foregroundWindowHandle = GetForegroundWindow();
            Models.Win32.Rect rect;

            GetWindowRect(foregroundWindowHandle, out rect);
            return rect;
        }
    }
}
