namespace topdown1;

public enum Direction4
{
    Down,
    Left,
    Up,
    Right,
}

public static class Direction4Extensions
{
    public static Direction4 Previous(this Direction4 direction)
    {
        switch (direction)
        {
            case Direction4.Down:
                return Direction4.Right;
            default:
                return direction - 1;
        }
    }
    public static Direction4 Next(this Direction4 direction)
    {
        switch (direction)
        {
            case Direction4.Right:
                return Direction4.Down;
            default:
                return direction + 1;
        }
    }
}