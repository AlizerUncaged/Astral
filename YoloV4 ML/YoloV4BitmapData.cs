using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using System.Drawing;
using Microsoft.ML.Transforms.Image;

namespace YoloV4
{
    public class YoloV4BitmapData
    {
        [ColumnName("bitmap")]
        [ImageType(416, 416)]
        public Bitmap Image { get; set; }

        [ColumnName("width")]
        public float ImageWidth => Image.Width;

        [ColumnName("height")]
        public float ImageHeight => Image.Height;
    }
}
