using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

public class GhostState_Chasing : GhostState
{
    public GhostState_Chasing(Ghost ghost) : base(ghost)
    {
    }

    public override void Enter()
    {
        m_Ghost.Renderer.SetColor(Color.Red);
        ViewRangeSquared = MathF.Pow(MapGrid.TileSize * 9, 2);
        PathSize = 5;

        RefreshPath();
    }

    protected override void Chase()
    {
        // do nothign
    }

    public override void RefreshPath()
    {
        Player player = RoundInfo.GetPlayer(0);

        Point currentCoord = MapGrid.PositionToGridCoordinate(m_Ghost.Position);
        Point playerCoord = MapGrid.PositionToGridCoordinate(player.CenterPosition);
        MapGrid.GetPathTo(currentCoord, playerCoord, PathSize, out List<Point> newPath);

        m_Ghost.Path.Clear();
        foreach (var coord in newPath)
            m_Ghost.Path.Add(MapGrid.GridCoordinateToPosition(coord));
    }
}