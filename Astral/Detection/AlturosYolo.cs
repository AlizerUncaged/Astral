using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Detection
{
    [Obsolete("Project moved to https://github.com/maalik0786/FastYolo use FastYolo instead.", true)]
    public class AlturosYolo : IService, IDetectorService
    {
        public event EventHandler<IEnumerable>? PredictionReceived;
    }
}
