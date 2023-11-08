using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

public class GhostState_Roaming : GhostState
{
    public override bool IsCollidable => true;

    public GhostState_Roaming(Ghost ghost) : base(ghost)
    {

    }

    public override void Enter()
    {
        m_Ghost.Renderer.SetColor(Color.White);
        ViewRangeSquared = MathF.Pow(MapGrid.TileSize * 4, 2);

        if (m_Ghost.Path.Count < 2)
            RefreshPath();
        // only refreshes path if really needed. it's okay if roaming state uses the rest of path calculated in another state
    }

    protected override void Roam()
    {
        // do nothing
    }
}