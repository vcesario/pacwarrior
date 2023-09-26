
using Microsoft.Xna.Framework;

public struct BoundingBox
{
    public Point TopLeft;
    public int Width;
    public int Height;

    public int X => TopLeft.X;
    public int Y => TopLeft.Y;
    public Point TopRight => new Point(X + Width, Y);
    public Point BottomLeft => new Point(X, Y + Height);
    public Point BottomRight => new Point(X + Width, Y + Height);

    public BoundingBox(int x, int y, int width, int height)
    {
        TopLeft = new Point(x, y);
        Width = width;
        Height = height;
    }
}