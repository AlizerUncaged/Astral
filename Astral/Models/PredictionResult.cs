using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models
{
    public class PredictionResult : BoundsBase, IBounds
    {
        public PredictionResult(string? label, float score, Point location, Size size, int? labelIndex)
        {
            Label = label;
            Score = score;
            Location = location;
            Size = size;
            LabelIndex = labelIndex;
        }

        /// <summary>
        /// The object's identity id which should be
        /// the same of every frame.
        /// </summary>
        public int? ObjectId { get; set; }

        public int? LabelIndex { get; }

        public string? Label { get; }

        public float Score { get; }

        public override Point Location { get; }

        public override Size Size { get; }

        // Additional data along with this result,
        // this is a temporary fix until we found
        // a more beautiful way of sending additional
        // information along with the bitmap.
        public object? Tag { get; set; }
    }
}
