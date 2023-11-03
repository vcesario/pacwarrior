using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

public abstract class GhostState : IState
{
    protected Ghost m_Ghost;
    protected Player m_Player;

    private float m_MovePercent;

    public GhostState(Ghost ghost, Player player)
    {
        m_Ghost = ghost;
        m_Player = player;
    }

    public void Update(GameTime gameTime)
    {
        // move ghosts
        if (m_MovePercent >= 1)
        {
            m_Ghost.SetPosition(m_Ghost.Path[1]);
            m_Ghost.Path.RemoveAt(0);

            RefreshPath(m_Player);

            // set next direction
            Direction4 nextDirection = MapGrid.PositionToGridCoordinate(m_Ghost.Path[1] - m_Ghost.Path[0]).ToDirection4();
            m_Ghost.SetDirection(nextDirection);
        }
        else
        {
            Vector2 newPosition = Vector2.Lerp(m_Ghost.Path[0].ToVector2(), m_Ghost.Path[1].ToVector2(), m_MovePercent);
            m_Ghost.SetPosition(newPosition.ToPoint());
        }
    }

    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }

    public void SetMovePercent(float value)
    {
        m_MovePercent = value;
    }

    public virtual void RefreshPath(Player player)
    {
        bool needNewPath = m_Ghost.Path.Count < 2;

        if (!needNewPath)
            return;

        Point currentCoord = MapGrid.PositionToGridCoordinate(m_Ghost.Position);
        Direction4 currentDirection = m_Ghost.CurrentDirection;

        MapGrid.GetRandomPath(currentCoord, currentDirection, GhostAI.RoamPathSize, out List<Point> newPath);

        m_Ghost.Path.Clear();
        foreach (var coord in newPath)
            m_Ghost.Path.Add(MapGrid.GridCoordinateToPosition(coord));
    }
}