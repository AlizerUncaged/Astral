using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models
{
    /// <summary>
    /// The current program's status.
    /// </summary>
    public class ProgramStatus : IUtility
    {
        public bool IsClosing { get; set; }
    }
}
