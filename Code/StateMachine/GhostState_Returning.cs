using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

public class GhostState_Returning : GhostState
{
    public GhostState_Returning(Ghost ghost) : base(ghost)
    {
    }

    public override void Update(GameTime gameTime, float movePercent)
    {
        // move ghosts
        if (movePercent >= 1)
        {
            // end current step
            m_Ghost.SetPosition(m_Ghost.Path[1]);
            m_Ghost.Path.RemoveAt(0);

            // recalculate
            if (m_Ghost.Path.Count < 2)
            {
                Recharge();
            }
            else
            {
                // begin next step
                // set next direction
                Direction4 nextDirection = MapGrid.PositionToGridCoordinate(m_Ghost.Path[1] - m_Ghost.Path[0]).ToDirection4();
                m_Ghost.SetDirection(nextDirection);
            }
        }
        else
        {
            // @TODO: shared movePercent value causes movement hiccups now since ghosts can be killed mid "step"
            Vector2 newPosition = Vector2.Lerp(m_Ghost.Path[0].ToVector2(), m_Ghost.Path[1].ToVector2(), movePercent);
            m_Ghost.SetPosition(newPosition.ToPoint());
        }
    }

    public override void Enter()
    {
        m_Ghost.Renderer.SetColor(Color.MediumPurple);
        ViewRangeSquared = 0;
        PathSize = int.MaxValue;

        RefreshPath();
    }

    public override void Die()
    {
        // do nothing
    }

    public override void RefreshPath()
    {
        Point currentCoord = MapGrid.PositionToGridCoordinate(m_Ghost.Position);
        MapGrid.GetPathToAny(currentCoord, GhostAI.SpawnCoords, PathSize, out List<Point> newPath);

        m_Ghost.Path.Clear();
        foreach (var coord in newPath)
            m_Ghost.Path.Add(MapGrid.GridCoordinateToPosition(coord));
    }

    private void Recharge()
    {
        m_Ghost.SetState(new GhostState_Recharging(m_Ghost));
    }
}