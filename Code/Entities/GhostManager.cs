using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class GhostManager
{
    private static List<Ghost> m_Ghosts;
    public static IEnumerable<Ghost> Ghosts => m_Ghosts;
    private static Tuple<Point, Point>[] m_GhostMoveCoords;

    private static float m_GhostSpeed = 100;
    private static float m_MovementDuration;
    private static TimeSpan m_MovementStartTime;

    // ghost view range = 100 (circle radius)
    // 


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
        int initialGhostAmount = 10;
        for (int i = 0; i < initialGhostAmount; i++)
        {
            Ghost newGhost = new Ghost(MapGrid.GridCoordinateToPosition(walkables[i]));
            m_Ghosts.Add(newGhost);
        }
        m_GhostMoveCoords = new Tuple<Point, Point>[m_Ghosts.Count];

        // ** calculate movement duration
        m_MovementDuration = MapGrid.TileSize / m_GhostSpeed; // approximate formula to match player's
        // the correct formula would be the following (calculated at every frame)
        //              m_MovementDuration = (MapGrid.TileSize / (m_GhostSpeed * frameDuration)) * frameDuration;
        // also fractioned values would have to be stored, meaning that more than one "next coordinate" would have
        // to be calculated ahead of time to prevent movement stuttering... and probably other stuff, but I decided
        // not to think about it :)
        // **

        CalculateNextMovement();
    }

    public static void UpdateBrain()
    {
        // move ghosts
        float elapsedTime = (float)(GameScreen.RoundDuration - m_MovementStartTime).TotalSeconds;
        float elapsedPercent = elapsedTime / m_MovementDuration;

        if (elapsedPercent >= 1)
        {
            for (int i = 0; i < m_Ghosts.Count; i++)
                m_Ghosts[i].SetPosition(m_GhostMoveCoords[i].Item2);

            CalculateNextMovement();
            m_MovementStartTime = GameScreen.RoundDuration;
        }
        else
        {
            // move all ghosts
            for (int i = 0; i < m_Ghosts.Count; i++)
            {
                Vector2 newPosition = Vector2.Lerp(m_GhostMoveCoords[i].Item1.ToVector2(), m_GhostMoveCoords[i].Item2.ToVector2(), elapsedPercent);
                m_Ghosts[i].SetPosition(newPosition.ToPoint());
            }
        }
    }

    private static void CalculateNextMovement()
    {
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
                GetRoamCoord(gridCoordinate, direction, out Point nextCoordinate, out Direction4 nextDirection);
                m_Ghosts[i].SetDirection(nextDirection);
                m_GhostMoveCoords[i] = new Tuple<Point, Point>(m_Ghosts[i].Position, MapGrid.GridCoordinateToPosition(nextCoordinate));
            }
            else // else, make ghost chase player
            {
                Point nextCoordinate = GetChaseCoord();
                m_GhostMoveCoords[i] = new Tuple<Point, Point>(m_Ghosts[i].Position, MapGrid.GridCoordinateToPosition(nextCoordinate));
            }
        }
    }

    private static void GetRoamCoord(Point currentCoordinate, Direction4 currentDirection, out Point nextCoordinate, out Direction4 nextDirection)
    {
        Direction4 leftDirection = currentDirection.Previous();
        Direction4 rightDirection = currentDirection.Next();
        Point frontCoord = currentCoordinate + currentDirection.ToCoordinate();
        Point leftCoord = currentCoordinate + leftDirection.ToCoordinate();
        Point rightCoord = currentCoordinate + rightDirection.ToCoordinate();

        List<Point> walkable = new List<Point>();
        if (!MapGrid.IsTileAWall(frontCoord.X, frontCoord.Y))
            walkable.Add(frontCoord);
        if (!MapGrid.IsTileAWall(leftCoord.X, leftCoord.Y))
            walkable.Add(leftCoord);
        if (!MapGrid.IsTileAWall(rightCoord.X, rightCoord.Y))
            walkable.Add(rightCoord);

        // here I could make sure that walkable has at least 1 element, but since the level loops on itself,
        // this will always be true so I'll save myself this check... unless there's an error on the map editing side  x)
        nextCoordinate = walkable[GameStartup.RandomGenerator.Next(walkable.Count)];
        if (nextCoordinate == frontCoord)
            nextDirection = currentDirection;
        else if (nextCoordinate == leftCoord)
            nextDirection = leftDirection;
        else //if (nextCoordinate == rightCoord)
            nextDirection = rightDirection;
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