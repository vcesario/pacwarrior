using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class TextureManager
{
    private static Texture2D _characterTex;
    public static Texture2D CharacterTex
    {
        get
        {
            if (_characterTex == null)
                _characterTex = GameStartup.ContentManager.Load<Texture2D>("textures/tilemap");

            return _characterTex;
        }
    }

    public static int CharacterTexTileSize = 16;
    private static Point GhostTexCoords = new Point(1, 10);
    private static Point PlayerTexCoords = new Point(0, 8);

    public static Rectangle GhostTexSourceRect = new Rectangle(
        TextureManager.CharacterTexTileSize * GhostTexCoords.X,
        TextureManager.CharacterTexTileSize * GhostTexCoords.Y,
        TextureManager.CharacterTexTileSize,
        TextureManager.CharacterTexTileSize
    );

    public static Rectangle PlayerTexSourceRect = new Rectangle(
        TextureManager.CharacterTexTileSize * PlayerTexCoords.X,
        TextureManager.CharacterTexTileSize * PlayerTexCoords.Y,
        TextureManager.CharacterTexTileSize,
        TextureManager.CharacterTexTileSize
    );
}