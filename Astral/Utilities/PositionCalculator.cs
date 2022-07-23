using Astral.Models.Configurations;
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

        public PositionCalculator(ScreenConfig screenConfig)
        {
            this.screenConfig = screenConfig;
        }


        /// <summary>
        /// Recalculates the object's position on the desktop based on the downscale
        /// done to the image.
        /// </summary>
        /// <param name="startingPoint">The position where to start, if the object is from a
        /// window, then this should be the window's position on the desktop, if there is no
        /// window, this can be set to Point.Empty.</param>
        /// <param name="scaledObjectLocation">The position of the object.</param>
        /// <param name="scaledObjectSize">The scaled size of the object, if downscale is 1 or none
        /// then it is so.</param>
        /// <returns>The object's real position on the desktop.</returns>
        public PointF RecalculateObjectPosition(Point startingPoint, Point scaledObjectLocation, Size scaledObjectSize)
        {
            var normalizedObjectX = scaledObjectLocation.X / screenConfig.Downscale;
            var normalizedObjectY = scaledObjectLocation.Y / screenConfig.Downscale;
            var normalizedObjectWidth = scaledObjectSize.Width / screenConfig.Downscale;
            var normalizedObjectHeight = scaledObjectSize.Height / screenConfig.Downscale;

            var centeredLocation = new PointF(startingPoint.X + normalizedObjectX + (normalizedObjectWidth / 2),
                startingPoint.Y + normalizedObjectY + (normalizedObjectHeight / 2));

            return centeredLocation;
        }

        ///// <summary>
        ///// If the monitor is scaled in the display settings, this function
        ///// gets the actual physical location.
        ///// </summary>
        //public PointF CalculatePhysicalLocationFromScaled(PointF monitorScaledPosition)
        //{
        //    var scaledPoint = Point.Round(monitorScaledPosition);

        //    var effectiveDpi = monitorInfo.GetMonitorDpiFromPoint((WinPoint)scaledPoint, MonitorDpiType.EFFECTIVE_DPI);

        //    // var rawDpi = monitorInfo.GetMonitorDpiFromPoint((WinPoint)scaledPoint, MonitorDpiType.RAW_DPI);

        //    var dpiScaling = Math.Round((double)effectiveDpi / 96d, 2);

        //    var scaledX = scaledPoint.X * dpiScaling;
        //    var scaledY = scaledPoint.Y * dpiScaling;

        //    var scaledPosition = new Point((int)scaledX, (int)scaledY);

        //    Console.WriteLine($"Physical point? {scaledPosition}, scaled: {monitorScaledPosition}");

        //    return scaledPosition;
        //}

    }
}
