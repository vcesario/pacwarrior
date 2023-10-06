using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class TextureManager
{
    private static Texture2D _mainTex;
    public static Texture2D MainTex
    {
        get
        {
            if (_mainTex == null)
                _mainTex = GameStartup.ContentManager.Load<Texture2D>("textures/tilemap");

            return _mainTex;
        }
    }

    public static int CharacterTexTileSize = 16;
    private static Point GhostTexCoords = new Point(1, 10);
    private static Point PlayerTexCoords = new Point(0, 8);
    private static Point Coin1TexCoords = new Point(6, 9);
    private static Point Coin2TexCoords = new Point(7, 9);
    private static Point Coin3TexCoords = new Point(8, 9);

    public static Rectangle GhostTexSourceRect = new Rectangle(
        CharacterTexTileSize * GhostTexCoords.X,
        CharacterTexTileSize * GhostTexCoords.Y,
        CharacterTexTileSize,
        CharacterTexTileSize
    );

    public static Rectangle PlayerTexSourceRect = new Rectangle(
        CharacterTexTileSize * PlayerTexCoords.X,
        CharacterTexTileSize * PlayerTexCoords.Y,
        CharacterTexTileSize,
        CharacterTexTileSize
    );

    public static Rectangle Coin1TexSourceRect = new Rectangle(
        CharacterTexTileSize * Coin1TexCoords.X,
        CharacterTexTileSize * Coin1TexCoords.Y,
        CharacterTexTileSize,
        CharacterTexTileSize
    );

    public static Rectangle Coin2TexSourceRect = new Rectangle(
        CharacterTexTileSize * Coin2TexCoords.X,
        CharacterTexTileSize * Coin2TexCoords.Y,
        CharacterTexTileSize,
        CharacterTexTileSize
    );

    public static Rectangle Coin3TexSourceRect = new Rectangle(
        CharacterTexTileSize * Coin3TexCoords.X,
        CharacterTexTileSize * Coin3TexCoords.Y,
        CharacterTexTileSize,
        CharacterTexTileSize
    );
}