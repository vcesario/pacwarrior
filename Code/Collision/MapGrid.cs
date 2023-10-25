using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LDtk;
using LDtk.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class MapGrid
{
    public static int TileSize = -1;
    public static Point GridSize = Point.Zero;
    public static Point MapSizeInPixels => GridSize.Scale(TileSize);

    public static bool[] CollisionMap;

    private static LDtkRenderer m_Renderer;
    private static LDtkLevel m_Level;

    private static List<Point> _walkableCoordinates;
    public static IEnumerable<Point> WalkableCoordinates
    {
        get
        {
            if (_walkableCoordinates == null)
            {
                _walkableCoordinates = new List<Point>();
                for (int j = 0; j < GridSize.Y; j++)
                {
                    for (int i = 0; i < GridSize.X; i++)
                    {
                        if (!IsTileAWall(i, j))
                            _walkableCoordinates.Add(new Point(i, j));
                    }
                }
            }

            return _walkableCoordinates;
        }
    }

    public static void Initialize(LDtkLevel level, LDtkRenderer levelRenderer)
    {
        // setup grid dimensions
        TileSize = level.LayerInstances[0]._GridSize;
        GridSize.X = level.LayerInstances[0]._CWid;
        GridSize.Y = level.LayerInstances[0]._CHei;

        // initialize collision map
        LayerInstance layerInstance = level.LayerInstances.First(_layerInstance => _layerInstance._Identifier.Equals("Collision_layer"));
        CollisionMap = new bool[layerInstance.IntGridCsv.Length];
        for (int i = 0; i < layerInstance.IntGridCsv.Length; i++)
        {
            CollisionMap[i] = layerInstance.IntGridCsv[i] == 1;
        }

        // initialize renderer
        m_Renderer = levelRenderer;
        m_Renderer.PrerenderLevel(level);

        m_Level = level;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        m_Renderer.RenderPrerenderedLevel(m_Level);

        if (GameStartup.DebugEnabled)
        {
            for (int j = 0; j < GridSize.Y; j++)
            {
                for (int i = 0; i < GridSize.X; i++)
                {
                    if (!IsTileAWall(i, j))
                        continue;

                    Rectangle tileRect = new Rectangle(i * TileSize, j * TileSize, TileSize - 1, TileSize - 1);
                    Primitives2D.DrawRectangle(spriteBatch, tileRect, Color.Green);
                }
            }
        }
    }

    /// <summary>
    /// Returns the top-left pixel of that grid coordinate.
    /// </summary>
    public static Point GridCoordinateToPosition(Point gridCoordinate)
    {
        Point pixel = new Point(gridCoordinate.X * TileSize, gridCoordinate.Y * TileSize);
        return pixel;
    }
    public static Point PositionToGridCoordinate(Point position)
    {
        Point gridCoordinate = new Point(position.X / TileSize, position.Y / TileSize);
        return gridCoordinate;
    }

    public static bool IsTileAWall(int gridX, int gridY)
    {
        bool isWall = CollisionMap[gridY * GridSize.X + gridX];
        return isWall;
    }

    public static Point ToCoordinate(this Direction4 direction)
    {
        switch (direction)
        {
            case Direction4.Down:
                return new Point(0, 1);
            case Direction4.Left:
                return new Point(-1, 0);
            case Direction4.Right:
                return new Point(1, 0);
            case Direction4.Up:
                return new Point(0, -1);
            default:
                return Point.Zero;
        }
    }

    public static Direction4 ToDirection4(this Point point)
    {
        if (point.X > 0)
            return Direction4.Right;

        if (point.X < 0)
            return Direction4.Left;

        if (point.Y > 0)
            return Direction4.Down;

        return Direction4.Up;
    }

    public static IEnumerable<Point> Neighbors(this Point point)
    {
        Point top = point + Direction4.Up.ToCoordinate();
        Point left = point + Direction4.Left.ToCoordinate();
        Point right = point + Direction4.Right.ToCoordinate();
        Point down = point + Direction4.Down.ToCoordinate();

        return new Point[] { down, left, top, right };
    }

    public static void GetRandomPath(Point sourceCoord, Direction4 sourceDirection, int pathSize, out List<Point> result)
    {
        result = new List<Point>() { sourceCoord };

        Direction4 currentDirection = sourceDirection;

        while (result.Count < pathSize)
        {
            Point currentCoord = result[result.Count - 1];

            Direction4 leftDirection = currentDirection.Previous();
            Direction4 rightDirection = currentDirection.Next();
            Point frontCoord = currentCoord + currentDirection.ToCoordinate();
            Point leftCoord = currentCoord + leftDirection.ToCoordinate();
            Point rightCoord = currentCoord + rightDirection.ToCoordinate();

            List<Point> walkable = new List<Point>();
            if (!IsTileAWall(frontCoord.X, frontCoord.Y))
                walkable.Add(frontCoord);
            if (!IsTileAWall(leftCoord.X, leftCoord.Y))
                walkable.Add(leftCoord);
            if (!IsTileAWall(rightCoord.X, rightCoord.Y))
                walkable.Add(rightCoord);

            // here I could make sure that walkable has at least 1 element, but since the level loops on itself,
            // this will always be true so I'll save myself this check... unless there's an error on the map editing side  x)
            Point nextCoord = walkable[GameStartup.RandomGenerator.Next(walkable.Count)];

            // update currentDirection for the next step
            if (nextCoord == rightCoord)
                currentDirection = rightDirection;
            else if (nextCoord == leftCoord)
                currentDirection = leftDirection;
            //else if (nextCoord == currentCoord)
            // keep direction

            result.Add(nextCoord);
        }
    }
    public static void GetPathTo(Point sourceCoord, Point targetCoord, int pathSize, out List<Point> result)
    {
        result = new List<Point>() { sourceCoord };

        Queue<Point> frontier = new Queue<Point>();
        frontier.Enqueue(sourceCoord);
        Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>() { { sourceCoord, default } };

        // find path to target
        while (frontier.Count > 0)
        {
            Point current = frontier.Dequeue();
            if (current == targetCoord)
                break;

            foreach (var neighbor in current.Neighbors())
            {
                if (IsTileAWall(neighbor.X, neighbor.Y))
                    continue;

                if (!cameFrom.ContainsKey(neighbor))
                {
                    frontier.Enqueue(neighbor);
                    cameFrom.Add(neighbor, current);
                }
            }
        }

        // building whole path
        Stack<Point> path = new Stack<Point>();
        path.Push(targetCoord);
        while (cameFrom[path.Peek()] != sourceCoord)
        {
            Point previousCoord = cameFrom[path.Peek()];
            path.Push(previousCoord);
        }

        // getting path up to 'pathSize' steps
        while (result.Count < pathSize && path.Count > 0)
        {
            Point nextCoord = path.Pop();
            result.Add(nextCoord);
        }
    }
    public static void GetPathAwayFrom(Point sourceCoord, Point targetCoord, int pathSize, out List<Point> result)
    {
        result = new List<Point>() { sourceCoord };

        List<Point> frontier = new List<Point>() { sourceCoord };
        List<int> pathSizes = new List<int>() { getPathSize(sourceCoord) };
        Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>() { { sourceCoord, default } };
        List<Point> farthestCoords = new List<Point>();
        Point current = default;

        while (frontier.Count > 0)
        {
            // select next coord from frontier (pick a random one from the highest heuristic)
            farthestCoords.Clear();
            int highestSize = int.MinValue;
            for (int i = 0; i < frontier.Count; i++)
            {
                int thisPathSize = pathSizes[i];
                if (thisPathSize == highestSize)
                {
                    farthestCoords.Add(frontier[i]);
                }
                else if (thisPathSize > highestSize)
                {
                    farthestCoords.Clear();
                    highestSize = thisPathSize;
                    farthestCoords.Add(frontier[i]);
                }
                else // thisPathSize < highestSize
                {
                    // do nothing
                }
            }

            current = farthestCoords[GameStartup.RandomGenerator.Next(farthestCoords.Count)];
            int index = frontier.IndexOf(current);
            frontier.Remove(current);
            if (highestSize == pathSize) // if heuristic is equal to path size
                break;

            foreach (var neighbor in current.Neighbors())
            {
                if (IsTileAWall(neighbor.X, neighbor.Y))
                    continue;

                if (!cameFrom.ContainsKey(neighbor))
                {
                    frontier.Add(neighbor);
                    cameFrom.Add(neighbor, current);

                    pathSizes.Add(getPathSize(neighbor));
                }
            }
        }

        // building whole path
        Stack<Point> path = new Stack<Point>();
        path.Push(targetCoord);
        while (cameFrom[path.Peek()] != sourceCoord)
        {
            Point previousCoord = cameFrom[path.Peek()];
            path.Push(previousCoord);
        }

        // getting path up to 'pathSize' steps
        while (result.Count < pathSize && path.Count > 0)
        {
            Point nextCoord = path.Pop();
            result.Add(nextCoord);
        }

        // heuristic function
        int getPathSize(Point fromCoord)
        {
            int xDiff = (int)MathF.Abs(fromCoord.X - targetCoord.X);
            int yDiff = (int)MathF.Abs(fromCoord.Y - targetCoord.Y);
            return xDiff + yDiff + 1;
        }
    }
}