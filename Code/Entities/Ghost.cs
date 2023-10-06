using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class Ghost
{
    // private BoundingBox m_TexBox;
    // private Texture2D m_Tex;

    public Direction4 CurrentDirection
    { get; private set; }

    public Point Position => Renderer.Box.TopLeft;

    public TexRenderer Renderer;

    public Ghost(Point position)
    {
        Renderer = new TexRenderer(TextureManager.CharacterTex, TextureManager.GhostTexSourceRect, position);
        // m_TexBox = new BoundingBox(position.X, position.Y, TextureManager.CharacterTexTileSize, TextureManager.CharacterTexTileSize);
        // m_Tex = TextureManager.CharacterTex;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Renderer.Draw(spriteBatch);
        // spriteBatch.Draw(m_Tex, new Vector2(m_TexBox.Left, m_TexBox.Top), TextureManager.GhostTexSourceRect, Color.White);

        if (GameStartup.DebugEnabled)
        {
            spriteBatch.DrawRectangle(Renderer.Box.Rect, Color.MediumPurple);
        }
    }

    public void SetPosition(Point position)
    {
        Renderer.SetPosition(position);
        // m_TexBox.Top = position.Y;
        // m_TexBox.Left = position.X;
    }

    public void SetDirection(Direction4 direction)
    {
        CurrentDirection = direction;
    }

    public BoundingBox GetColliderBox()
    {
        return Renderer.Box; // @TODO: add a proper hit box maybe, instead of the graphics one
    }
}