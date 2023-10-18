using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class TextureManager
{
    private static Texture2D _mainTex;
    public static Texture2D MainSheet
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
    private static Point CoinTexCoords = new Point(6, 10);
    private static Point PowerUpTexCoords = new Point(7, 9);

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

    public static Rectangle CoinTexSourceRect = new Rectangle(
        CharacterTexTileSize * CoinTexCoords.X,
        CharacterTexTileSize * CoinTexCoords.Y,
        CharacterTexTileSize,
        CharacterTexTileSize
    );

    public static Rectangle PowerUpTexSourceRect = new Rectangle(
        CharacterTexTileSize * PowerUpTexCoords.X,
        CharacterTexTileSize * PowerUpTexCoords.Y,
        CharacterTexTileSize,
        CharacterTexTileSize
    );
}