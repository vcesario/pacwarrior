
using Microsoft.Xna.Framework;

public static class Extensions
{
    public static Point Scale(this Point point, int value)
    {
        point.X *= value;
        point.Y *= value;
        return point;
    }
    public static Point Scale(this Point point, float value)
    {
        point.X = (int)(point.X * value);
        point.Y = (int)(point.Y * value);
        return point;
    }
}