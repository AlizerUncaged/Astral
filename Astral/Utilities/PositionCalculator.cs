using Astral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class PositionCalculator : IUtility
    {
        private readonly ScreenConfig screenConfig;

        public PositionCalculator(ScreenConfig screenConfig) =>
            this.screenConfig = screenConfig;


        /// <summary>
        /// Recalculates the object's position on the desktop based on the downscale
        /// done to the image.
        /// </summary>
        /// <param name="startingPoint">The position where to start, if the object is from a
        /// window, then this should be the window's position on the desktop, if there is no
        /// window, this can be set to Point.Empty.</param>
        /// <param name="objectLocation">The position of the object.</param>
        /// <param name="objectSize">The scaled size of the object, if downscale is 1 or none
        /// then it is so.</param>
        /// <returns>The object's real position on the desktop.</returns>
        public PointF RecalculateObjectPosition(Point startingPoint, Point objectLocation, Size objectSize)
        {
            var normalizedObjectX = objectLocation.X / screenConfig.Downscale;
            var normalizedObjectY = objectLocation.Y / screenConfig.Downscale;
            var normalizedObjectWidth = objectSize.Width / screenConfig.Downscale;
            var normalizedObjectHeight = objectSize.Height / screenConfig.Downscale;

            var centeredLocation = new PointF(startingPoint.X + normalizedObjectX + (normalizedObjectWidth / 2),
                startingPoint.Y + normalizedObjectY + (normalizedObjectHeight / 2));

            return centeredLocation;
        }

    }
}
