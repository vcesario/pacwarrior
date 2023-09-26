
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

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

    public static void RandomSort<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomI = GameStartup.RandomGenerator.Next(0, list.Count);
            (list[i], list[randomI]) = (list[randomI], list[i]);
        }
    }
}