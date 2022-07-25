using Astral.Curses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    /// <summary>
    /// A class that helps choose which
    /// entity to pick.
    /// </summary>
    public class EntityPicker : IUtility
    {
        private readonly LocalMouseControl mouseControl;

        public EntityPicker(Curses.LocalMouseControl mouseControl)
        {
            this.mouseControl = mouseControl;
        }

        /// <summary>
        /// Gets the nearest entity based on mouse location
        /// else just gives the first element in the bounds
        /// list.
        /// </summary>
        public BoundsBase? GetCurrentEntityFocus(IEnumerable<BoundsBase> bounds)
        {
            var nearest = bounds.FirstOrDefault(x => x.IsInBounds(mouseControl.MouseLocation));
            return nearest is { } ? nearest : bounds.FirstOrDefault();
        }
    }
}
