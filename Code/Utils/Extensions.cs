
using Microsoft.Xna.Framework;

public static class Extensions
{
    public static Point Scale(this Point point, int value)
    {
        point.X *= value;
        point.Y *= value;
        return point;
    }
}