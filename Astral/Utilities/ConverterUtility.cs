using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class ConverterUtility : IUtility
    {
        public byte[] ImageToBytes(Bitmap bm)
        {
            using (var stream = new MemoryStream())
            {
                bm.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                return stream.ToArray();
            }
        }
    }
}
