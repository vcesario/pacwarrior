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

    public static List<Point> GetAllWalkableTiles()
    {
        List<Point> walkables = new List<Point>();
        for (int j = 0; j < GridSize.Y; j++)
        {
            for (int i = 0; i < GridSize.X; i++)
            {
                if (!IsTileAWall(i, j))
                    walkables.Add(new Point(i, j));
            }
        }

        return walkables;
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
}