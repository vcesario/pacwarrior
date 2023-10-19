using System;
using Microsoft.Xna.Framework;

namespace topdown1;

public abstract class Collectible
{
    public TexRenderer Renderer;
    public Point Position => Renderer.Box.TopLeft;

    protected abstract Rectangle m_SourceRect { get; }

    public event Action<Collectible> EventWasCollected;

    public Collectible(Point position)
    {
        Renderer = new TexRenderer(TextureManager.MainSheet, m_SourceRect, position);
    }

    public BoundingBox GetColliderBox()
    {
        return Renderer.Box;
    }

    public void Collect(Player player)
    {
        Internal_Collect(player);

        player.AddScore(1);
        EventWasCollected?.Invoke(this);
    }

    protected virtual void Internal_Collect(Player player) { }
}