using Microsoft.Xna.Framework;

namespace topdown1;

public class PlayerState_PoweredUp : PlayerState
{
    public PlayerState_PoweredUp(Player player) : base(player)
    {
    }

    protected override bool CanMove => throw new System.NotImplementedException();

    protected override bool CanCollectThings => throw new System.NotImplementedException();

    protected override bool CanCollideWithGhosts => throw new System.NotImplementedException();

    protected override bool CanPause => throw new System.NotImplementedException();
}