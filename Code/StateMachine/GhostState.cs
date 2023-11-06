using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

public abstract class GhostState : IState
{
    protected Ghost m_Ghost;

    protected float ViewRangeSquared { get; set; }
    protected int PathSize { get; set; } = 10;

    public GhostState(Ghost ghost)
    {
        m_Ghost = ghost;
    }

    public virtual void Update(GameTime gameTime, float movePercent)
    {
        // move ghosts
        if (movePercent >= 1)
        {
            // end current step
            m_Ghost.SetPosition(m_Ghost.Path[1]);
            m_Ghost.Path.RemoveAt(0);

            // recalculate
            RefreshState();

            // begin next step
            if (m_Ghost.Path.Count < 2)
                m_Ghost.State.RefreshPath(); // instead of simply calling "RefreshPath", I'm doing this to ensure that, in case RefreshState() triggers a change of state, it still calls the correct overriden RefreshPath

            // set next direction
            Direction4 nextDirection = MapGrid.PositionToGridCoordinate(m_Ghost.Path[1] - m_Ghost.Path[0]).ToDirection4();
            m_Ghost.SetDirection(nextDirection);
        }
        else
        {
            Vector2 newPosition = Vector2.Lerp(m_Ghost.Path[0].ToVector2(), m_Ghost.Path[1].ToVector2(), movePercent);
            m_Ghost.SetPosition(newPosition.ToPoint());
        }
    }

    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }

    private void RefreshState()
    {
        Player player = RoundInfo.GetPlayer(0);
        float squareDistance = Vector2.DistanceSquared(m_Ghost.Position.ToVector2(), player.Position.ToVector2());
        bool isPlayerInRange = squareDistance <= ViewRangeSquared;

        if (GameScreen.HasRoundEnded || !isPlayerInRange)
        {
            Roam();
        }
        else
        {
            if (player.State is PlayerState_Default)
            {
                Chase();
            }
            else if (player.State is PlayerState_PoweredUp)
            {
                Flee();
            }
            else
            {
                Roam();
            }
        }
    }

    protected virtual void Roam()
    {
        m_Ghost.SetState(new GhostState_Roaming(m_Ghost));
    }
    protected virtual void Chase()
    {
        m_Ghost.SetState(new GhostState_Chasing(m_Ghost));
    }
    protected virtual void Flee()
    {
        m_Ghost.SetState(new GhostState_Fleeing(m_Ghost));
    }
    public virtual void Die()
    {
        m_Ghost.SetState(new GhostState_Returning(m_Ghost));
    }

    public virtual void RefreshPath()
    {
        Point currentCoord = MapGrid.PositionToGridCoordinate(m_Ghost.Position);
        Direction4 currentDirection = m_Ghost.CurrentDirection;

        MapGrid.GetRandomPath(currentCoord, currentDirection, PathSize, out List<Point> newPath);

        m_Ghost.Path.Clear();
        foreach (var coord in newPath)
            m_Ghost.Path.Add(MapGrid.GridCoordinateToPosition(coord));
    }
}