namespace Astral.Curses;

public class LocalMouseControl : IUtility
{
    public Point MouseLocation
    {
        get => Cursor.Position;
        set => Cursor.Position =
            value;
    }
}

