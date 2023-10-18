using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

public class PlayerState_Default : PlayerState
{
    protected override bool CanMove => true;
    protected override bool CanCollectThings => true;
    protected override bool CanCollideWithGhosts => true;
    protected override bool CanPause => true;

    public PlayerState_Default(Player player) : base(player)
    {
    }
}
