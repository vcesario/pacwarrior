using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class GhostManager
{
    private static List<Ghost> m_Ghosts;
    private static Tuple<Point, Point>[] m_GhostMoveCoords;

    private static float m_GhostSpeed = 100;
    private static float m_MovementDuration;
    private static double m_MovementStartTime;

    private static bool m_IsMoving;

    public static void Initialize()
    {
        List<Point> walkables = MapGrid.GetAllWalkableTiles();

        // remove tiles in the lower half 
        for (int i = walkables.Count - 1; i >= 0; i--)
        {
            if (walkables[i].Y > MapGrid.GridSize.Y / 2.0f)
                walkables.RemoveAt(i);
        }

        // randomize positions
        walkables.RandomSort();

        // instantiate n ghosts
        m_Ghosts = new List<Ghost>();
        int initialGhostAmount = 3;
        for (int i = 0; i < initialGhostAmount; i++)
        {
            Ghost newGhost = new Ghost(MapGrid.GridCoordinateToPosition(walkables[i]));
            m_Ghosts.Add(newGhost);
        }
        m_GhostMoveCoords = new Tuple<Point, Point>[m_Ghosts.Count];

        // ** calculate movement duration
        m_MovementDuration = MapGrid.TileSize / m_GhostSpeed; // approximate formula to match player's
        // the right formula would be the following (calculated at every frame)
        //              m_MovementDuration = (MapGrid.TileSize / (m_GhostSpeed * frameDuration)) * frameDuration;
        // also fractioned values would have to be stored, meaning that more than one "next coordinate" would have
        // to be calculated ahead of time to prevent movement stuttering... and probably other stuff, but I decided
        // not to think about it :)
        // **
    }

    public static void UpdateBrain(GameTime gameTime)
    {
        if (m_IsMoving)
        {
            // move ghosts
            float elapsedTime = (float)(gameTime.TotalGameTime.TotalSeconds - m_MovementStartTime);
            float elapsedPercent = elapsedTime / m_MovementDuration;

            if (elapsedPercent >= 1)
            {
                for (int i = 0; i < m_Ghosts.Count; i++)
                    m_Ghosts[i].SetPosition(m_GhostMoveCoords[i].Item2);

                m_IsMoving = false;
            }
            else
            {
                for (int i = 0; i < m_Ghosts.Count; i++)
                {
                    Vector2 newPosition = Vector2.Lerp(m_GhostMoveCoords[i].Item1.ToVector2(), m_GhostMoveCoords[i].Item2.ToVector2(), elapsedPercent);
                    m_Ghosts[i].SetPosition(newPosition.ToPoint());
                }
            }
        }
        else
        {
            // calculate next movement
            if (m_GhostMoveCoords.Length != m_Ghosts.Count)
                m_GhostMoveCoords = new Tuple<Point, Point>[m_Ghosts.Count];

            for (int i = 0; i < m_Ghosts.Count; i++)
            {
                Point gridCoordinate = MapGrid.PositionToGridCoordinate(m_Ghosts[i].Position);
                Direction4 direction = m_Ghosts[i].CurrentDirection;

                // calculate distance from player
                // ...

                if (/**/true) // if distance large enough, make ghost roam
                {
                    Point nextCoordinate = GetRoamCoord(gridCoordinate, direction);
                    m_GhostMoveCoords[i] = new Tuple<Point, Point>(m_Ghosts[i].Position, MapGrid.GridCoordinateToPosition(nextCoordinate));
                }
                else // else, make ghost chase player
                {
                    Point nextCoordinate = GetChaseCoord();
                    m_GhostMoveCoords[i] = new Tuple<Point, Point>(m_Ghosts[i].Position, MapGrid.GridCoordinateToPosition(nextCoordinate));
                }
            }

            m_MovementStartTime = gameTime.TotalGameTime.TotalSeconds;
            m_IsMoving = true;
        }
    }

    private static Point GetRoamCoord(Point currentCoordinate, Direction4 direction)
    {
        return currentCoordinate + direction.ToCoordinate();
    }
    private static Point GetChaseCoord()
    {
        // ...
        return default;
    }

    public static void DrawGhosts(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < m_Ghosts.Count; i++)
        {
            m_Ghosts[i].Draw(spriteBatch);
        }
    }
}