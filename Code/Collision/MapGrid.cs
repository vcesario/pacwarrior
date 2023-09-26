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

    public static Point ConvertPixelToGrid(Point pxPosition)
    {
        Point gridCoordinate = new Point(pxPosition.X / TileSize, pxPosition.Y / TileSize);
        return gridCoordinate;
    }

    public static bool IsTileAWall(int gridX, int gridY)
    {
        bool isWall = CollisionMap[gridY * GridSize.X + gridX];
        return isWall;
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
}