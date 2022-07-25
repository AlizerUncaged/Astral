using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class SizeFormatProvider : IUtility
    {
        public string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
                dblSByte = bytes / 1024.0;

            return string.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        /// <summary>
        /// Gigabyte => Byte.
        /// </summary>
        public ulong GigabytesToBytes(ulong gigabytes) =>
            gigabytes * 1073741824;
    }
}
