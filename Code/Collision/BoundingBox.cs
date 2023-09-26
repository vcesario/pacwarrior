using Microsoft.Xna.Framework;

namespace topdown1;

public struct BoundingBox
{
    public int Top;
    public int Left;
    public int Width;
    public int Height;
    public float HalfWidth;
    public float HalfHeight;

    public Point TopLeft => new Point(Left, Top);
    public Point TopRight => new Point(Left + Width - 1, Top);
    public Point BottomLeft => new Point(Left, Top + Height - 1);
    public Point BottomRight => new Point(Left + Width - 1, Top + Height - 1);
    public Point CenterPosition
    {
        get
        {
            int x = (int)(Left + HalfWidth);
            int y = (int)(Top + HalfHeight);

            return new Point(x, y);
        }
    }
    public Rectangle Rect => new Rectangle(Left, Top, Width - 1, Height - 1);

    public BoundingBox(int x, int y, int width, int height)
    {
        Left = x;
        Top = y;
        Width = width;
        Height = height;
        HalfWidth = width * 0.5f;
        HalfHeight = height * 0.5f;
    }
}