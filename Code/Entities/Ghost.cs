using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class Ghost
{
    public Direction4 CurrentDirection
    { get; private set; }

    public Point Position => Renderer.Box.TopLeft;

    public TexRenderer Renderer;

    public GhostState State { get; private set; }

    // *** brain?
    public List<Point> Path;
    // ***

    public Ghost(Point position, Player player)
    {
        Renderer = new TexRenderer(TextureManager.MainSheet, TextureManager.GhostTexSourceRect, position);

        State = new GhostState_Roaming(this, player);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Renderer.Draw(spriteBatch);

        if (GameStartup.DebugEnabled)
        {
            spriteBatch.DrawRectangle(Renderer.Box.Rect, Color.MediumPurple);
        }
    }

    public void SetPosition(Point position)
    {
        Renderer.SetPosition(position);
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