namespace Astral.Curses;

public class MouseControl : IUtility
{
    public void MoveMouseTo(PointF position) =>
        MoveMouseTo(Point.Round(position));

    public void MoveMouseTo(Point position) =>
        Cursor.Position = position;
}

