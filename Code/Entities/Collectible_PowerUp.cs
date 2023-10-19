using Microsoft.Xna.Framework;

namespace topdown1;

public class Collectible_PowerUp : Collectible
{
    public Collectible_PowerUp(Point position) : base(position)
    {
    }

    protected override Rectangle m_SourceRect => TextureManager.PowerUpTexSourceRect;
    protected override void Internal_Collect(Player player)
    {
        player.State.ProcessPowerUpCollection();
    }
}