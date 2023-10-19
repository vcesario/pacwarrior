using Microsoft.Xna.Framework;

namespace topdown1;

public class Collectible_Coin : Collectible
{
    public Collectible_Coin(Point position) : base(position)
    {
    }

    protected override Rectangle m_SourceRect => TextureManager.CoinTexSourceRect;
}