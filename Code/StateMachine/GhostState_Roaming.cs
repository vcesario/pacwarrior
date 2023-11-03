using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

public class GhostState_Roaming : GhostState
{
    public GhostState_Roaming(Ghost ghost, Player player) : base(ghost, player)
    {

    }

    public override void Enter()
    {
        base.Enter();

        m_Ghost.Renderer.SetColor(Color.White);

        RefreshPath(null);
    }
}