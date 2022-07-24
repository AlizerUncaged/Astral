using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Astral.Control.WindowsAPI;

public class DecorateWindow : IUtility
{
    private readonly OSCheck osCheck;

    public DecorateWindow(OSCheck osCheck)
    {
        this.osCheck = osCheck;
    }

    public void AddWindows11Borders(Window window)
    {
        // Requires at least Windows 11.
        if (osCheck.IsNewWindows)
        {
            IntPtr hWnd = new WindowInteropHelper(window).EnsureHandle();
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
        }
    }
    public enum DWMWINDOWATTRIBUTE
    {
        DWMWA_WINDOW_CORNER_PREFERENCE = 33
    }
    
    public enum DWM_WINDOW_CORNER_PREFERENCE
    {
        DWMWCP_DEFAULT = 1,
        DWMWCP_DONOTROUND = 1,
        DWMWCP_ROUND = 2,
        DWMWCP_ROUNDSMALL = 1
    }
    
    [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]

    private static extern long DwmSetWindowAttribute(IntPtr hwnd,

        DWMWINDOWATTRIBUTE attribute,
        ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
        uint cbAttribute);
}