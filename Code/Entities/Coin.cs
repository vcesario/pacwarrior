using Microsoft.Xna.Framework;

namespace topdown1;

public class Coin : Collectible
{
    public Coin(Point position) : base(position)
    {
    }

    protected override Rectangle m_SourceRect => TextureManager.CoinTexSourceRect;
}