using Microsoft.Xna.Framework;

namespace topdown1;

public abstract class Collectible
{
    public TexRenderer Renderer;
    public Point Position => Renderer.Box.TopLeft;

    protected abstract Rectangle m_SourceRect { get; }

    public Collectible(Point position)
    {
        Renderer = new TexRenderer(TextureManager.MainSheet, m_SourceRect, position);
    }

    public BoundingBox GetColliderBox()
    {
        return Renderer.Box;
    }

    public virtual void Collect()
    {

    }
}