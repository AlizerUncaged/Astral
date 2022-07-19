using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models
{
    public class PredictionResult
    {
        public PredictionResult(string label, float score, PointF location, SizeF size, int labelIndex)
        {
            Label = label;
            Score = score;
            Location = location;
            Size = size;
            LabelIndex = labelIndex;
        }
        public int LabelIndex { get; }

        public string Label { get; }

        public float Score { get; }

        public PointF Location { get; }

        public SizeF Size { get; }


    }
}
