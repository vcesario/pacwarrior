using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

public class GhostState_Fleeing : GhostState
{

    public GhostState_Fleeing(Ghost ghost) : base(ghost)
    {
    }

    public override void Enter()
    {
        m_Ghost.Renderer.SetColor(Color.RoyalBlue);
        ViewRangeSquared = MathF.Pow(MapGrid.TileSize * 9, 2);
        PathSize = 4;

        RefreshPath();
    }

    protected override void Flee()
    {
        // do nothing
    }

    public override void RefreshPath()
    {
        Player player = RoundInfo.GetPlayer(0);

        Point currentCoord = MapGrid.PositionToGridCoordinate(m_Ghost.Position);
        Point playerCoord = MapGrid.PositionToGridCoordinate(player.Position);
        MapGrid.GetPathAwayFrom(currentCoord, playerCoord, PathSize, out List<Point> newPath);

        m_Ghost.Path.Clear();
        foreach (var coord in newPath)
            m_Ghost.Path.Add(MapGrid.GridCoordinateToPosition(coord));
    }
}