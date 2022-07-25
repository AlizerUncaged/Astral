using System;

namespace Astral.Control.WindowsAPI;

public class OSCheck : IUtility
{
    /// <summary>
    /// Returns true if the Windows version is higher than or equal to
    /// Windows 11.
    /// </summary>
    public bool IsNewWindows => Environment.OSVersion.Version.Build >= 22000;
}