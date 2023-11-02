
using System.Collections.Generic;
using LDtk;
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

    public static T[] GetEntities<T>(this LDtkWorld world) where T : new()
    {
        List<T> val = new List<T>();
        foreach (LDtkLevel level in world.Levels)
            val.AddRange(level.GetEntities<T>());

        return val.ToArray();
    }

    public static Color RandomColor()
    {
        return new Color(GameStartup.RandomGenerator.NextSingle(), GameStartup.RandomGenerator.NextSingle(), GameStartup.RandomGenerator.NextSingle());
    }
}