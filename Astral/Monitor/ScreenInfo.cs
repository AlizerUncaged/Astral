﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Monitor
{
    public class ScreenInfo
    {
        private readonly Screen _screen;
        // private readonly DEVMODE _device;

        private ScreenInfo(Screen screen/*, DEVMODE device */)
        {
            _screen = screen;
            // _device = device;
        }

        const int ENUM_CURRENT_SETTINGS = -1;


        //public Rectangle PhysicalBounds =>
        //    new(_device.dmPositionX, _device.dmPositionY, _device.dmPelsWidth,
        //        _device.dmPelsHeight);

        public Rectangle Bounds => _screen.Bounds;

        public string Name => _screen.DeviceName;

        public static ScreenInfo From(Screen screen)
        {
            //  var dm = new DEVMODE { dmSize = (short)Marshal.SizeOf(typeof(DEVMODE)) };
            // EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);

            return new ScreenInfo(screen/*, dm*/);
        }


        //[DllImport("user32.dll")]
        //public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        //[StructLayout(LayoutKind.Sequential)]
        //public struct DEVMODE
        //{
        //    private const int CCHDEVICENAME = 0x20;
        //    private const int CCHFORMNAME = 0x20;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        //    public string dmDeviceName;
        //    public short dmSpecVersion;
        //    public short dmDriverVersion;
        //    public short dmSize;
        //    public short dmDriverExtra;
        //    public int dmFields;
        //    public int dmPositionX;
        //    public int dmPositionY;
        //    public ScreenOrientation dmDisplayOrientation;
        //    public int dmDisplayFixedOutput;
        //    public short dmColor;
        //    public short dmDuplex;
        //    public short dmYResolution;
        //    public short dmTTOption;
        //    public short dmCollate;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        //    public string dmFormName;
        //    public short dmLogPixels;
        //    public int dmBitsPerPel;
        //    public int dmPelsWidth;
        //    public int dmPelsHeight;
        //    public int dmDisplayFlags;
        //    public int dmDisplayFrequency;
        //    public int dmICMMethod;
        //    public int dmICMIntent;
        //    public int dmMediaType;
        //    public int dmDitherType;
        //    public int dmReserved1;
        //    public int dmReserved2;
        //    public int dmPanningWidth;
        //    public int dmPanningHeight;
        //}
    }
}
