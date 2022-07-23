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
    public class AstralStatus : IUtility
    {
        /// <summary>
        /// True if program is about to close.
        /// </summary>
        public bool IsClosing => isClosing;

        private bool isClosing = false;

        public void CloseProgram() => isClosing = true;
    }
}
