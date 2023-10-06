using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class TexRenderer
{
    private BoundingBox m_TexBox;
    public BoundingBox Box => m_TexBox;
    private Texture2D m_Tex;
    private Rectangle m_TexRect;
    private Color m_SpriteColor;
    public Point Position => m_TexBox.TopLeft;

    public TexRenderer(Texture2D sourceTex, Rectangle sourceRect, Point startingPosition = default)
    {
        m_Tex = sourceTex;
        m_TexRect = sourceRect;
        m_TexBox = new BoundingBox(startingPosition.X, startingPosition.Y, sourceRect.Width, sourceRect.Height);

        m_SpriteColor = Color.White;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(m_Tex, Position.ToVector2(), m_TexRect, m_SpriteColor);
    }

    public void SetColor(Color newColor)
    {
        m_SpriteColor = newColor;
    }

    public void SetPosition(Point position)
    {
        m_TexBox.Left = position.X;
        m_TexBox.Top = position.Y;
    }
}