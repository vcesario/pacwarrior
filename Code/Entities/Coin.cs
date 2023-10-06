using Microsoft.Xna.Framework;

namespace topdown1;

public class Coin
{
    public TexRenderer Renderer;

    public Coin(Point position)
    {
        Renderer = new TexRenderer(TextureManager.MainTex, TextureManager.Coin1TexSourceRect, position);
    }

    public BoundingBox GetColliderBox()
    {
        return Renderer.Box;
    }
}