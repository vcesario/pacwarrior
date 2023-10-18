using Microsoft.Xna.Framework;

namespace topdown1;

public class PowerUp : Collectible
{
    public PowerUp(Point position) : base(position)
    {
    }

    protected override Rectangle m_SourceRect => TextureManager.PowerUpTexSourceRect;
}