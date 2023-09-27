using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class Ghost
{
    private BoundingBox m_TexBox;
    private Texture2D m_Tex;

    public Direction4 CurrentDirection
    { get; private set; }

    public Point Position => m_TexBox.TopLeft;

    public Ghost(Point position)
    {
        m_TexBox = new BoundingBox(position.X, position.Y, TextureManager.CharacterTexTileSize, TextureManager.CharacterTexTileSize);
        m_Tex = TextureManager.CharacterTex;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(m_Tex, new Vector2(m_TexBox.Left, m_TexBox.Top), TextureManager.GhostTexSourceRect, Color.White);
    }

    public void SetPosition(Point position)
    {
        m_TexBox.Top = position.Y;
        m_TexBox.Left = position.X;
    }
}