using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Runtime.InteropServices;

namespace Astral.Curses
{
    public class MouseControl : IUtility
    {
        [DllImport("User32.Dll")]
        private static extern long SetCursorPos(int x, int y);


        public void MoveMouseTo(PointF position) =>
            MoveMouseTo(Point.Round(position));

        public void MoveMouseTo(Point position) =>
            SetCursorPos(position.X, position.Y);
    }
}
