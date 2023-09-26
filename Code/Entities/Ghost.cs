using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class Ghost
{
    private BoundingBox m_TexBox;
    private Texture2D m_Tex;

    public Ghost(Point position)
    {
        m_TexBox = new BoundingBox(position.X, position.Y, TextureManager.CharacterTexTileSize, TextureManager.CharacterTexTileSize);
        m_Tex = TextureManager.CharacterTex;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(m_Tex, new Vector2(m_TexBox.Left, m_TexBox.Top), TextureManager.GhostTexSourceRect, Color.White);
    }
}