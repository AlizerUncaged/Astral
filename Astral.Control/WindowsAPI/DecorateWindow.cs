using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Astral.Control.WindowsAPI;

public class DecorateWindow : IUtility
{
    private readonly OSCheck osCheck;

    public DecorateWindow(OSCheck osCheck)=>
        this.osCheck = osCheck;
    

    public void AddWindows11Borders(Window window)
    {
        // Requires at least Windows 11.
        if (osCheck.IsNewWindows)
        {
            IntPtr hWnd = new WindowInteropHelper(window)
                .EnsureHandle();
            var attribute = Dwmwindowattribute
                .DwmwaWindowCornerPreference;
            var preference = DwmWindowCornerPreference
                .DwmwcpRound;
            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
        }
    }

    private enum Dwmwindowattribute
    {
        DwmwaWindowCornerPreference = 33
    }

    private enum DwmWindowCornerPreference
    {
        DwmwcpDefault = 1,
        DwmwcpDonotround = 1,
        DwmwcpRound = 2,
        DwmwcpRoundsmall = 1
    }
    
    [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]

    private static extern long DwmSetWindowAttribute(IntPtr hwnd,
        Dwmwindowattribute attribute,
        ref DwmWindowCornerPreference pvAttribute,
        uint cbAttribute);
}