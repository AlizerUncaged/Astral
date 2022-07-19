using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace Astral.Curses
{
    public class MouseControl : IUtility
    {
        public void MoveMouseTo(PointF position) =>
            Cursor.Position = Point.Round(position);
    }
}
