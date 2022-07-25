using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Control
{
    public interface IPage
    {
        /// <summary>
        /// Event whenever the current page is to be replaced
        /// by another page.
        /// </summary>
        event EventHandler<IPage> Replaced;
    }
}
