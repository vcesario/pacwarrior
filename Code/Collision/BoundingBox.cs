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

    public int Right => Left + Width - 1;
    public int Bottom => Top + Height - 1;
    public Point TopLeft => new Point(Left, Top);
    public Point TopRight => new Point(Right, Top);
    public Point BottomLeft => new Point(Left, Bottom);
    public Point BottomRight => new Point(Right, Bottom);
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

    public bool IsOverlapping(BoundingBox otherBox)
    {
        return Right >= otherBox.Left && Left <= otherBox.Right
        && Bottom >= otherBox.Top && Top <= otherBox.Bottom;
    }
}