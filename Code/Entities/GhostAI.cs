using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class GhostAI
{
    private static List<Ghost> m_Ghosts;
    public static IEnumerable<Ghost> Ghosts => m_Ghosts;
    private static List<List<Point>> m_GhostPaths;

    private enum GhostBehavior
    { Roaming = 0, Chasing = 1, Fleeing = 2 }
    private static List<GhostBehavior> m_GhostBehaviors;

    private static float m_GhostSquareRange_Roaming;
    private static float m_GhostSquareRange_ChasingOrFleeing;
    private static float m_GhostSpeed;
    private static float m_MovementDuration;
    private static TimeSpan m_MovementStartTime;

    private static int m_RoamPathSize;
    private static int m_ChasePathSize;
    private static int m_FleePathSize;
    private static int m_StartingGhostAmount;

    public static void Initialize()
    {
        // initialize balancing values
        m_GhostSpeed = 100;
        m_RoamPathSize = 10;
        m_ChasePathSize = 5;
        m_FleePathSize = 4;
        m_StartingGhostAmount = 5;
        m_GhostSquareRange_Roaming = MathF.Pow(MapGrid.TileSize * 4, 2);
        m_GhostSquareRange_ChasingOrFleeing = MathF.Pow(MapGrid.TileSize * 9, 2);

        // calculate viable positions for spawning ghosts
        List<Point> walkables = new List<Point>();

        // only consider tiles in the upper half 
        foreach (var coordinate in MapGrid.WalkableCoordinates)
            if (coordinate.Y <= MapGrid.GridSize.Y / 2.0f)
                walkables.Add(coordinate);

        // randomize positions
        walkables.RandomSort();

        // instantiate n ghosts
        m_Ghosts = new List<Ghost>(m_StartingGhostAmount);
        m_GhostPaths = new List<List<Point>>(m_StartingGhostAmount);
        m_GhostBehaviors = new List<GhostBehavior>(m_StartingGhostAmount);
        for (int i = 0; i < m_StartingGhostAmount; i++)
        {
            Ghost newGhost = new Ghost(MapGrid.GridCoordinateToPosition(walkables[i]));
            m_GhostPaths.Add(new List<Point>());
            m_GhostBehaviors.Add(GhostBehavior.Roaming);

            m_Ghosts.Add(newGhost);
        }

        // ** calculate movement duration
        m_MovementDuration = MapGrid.TileSize / m_GhostSpeed; // approximate formula to match player's
        // the correct formula would be the following (calculated at every frame)
        //              m_MovementDuration = (MapGrid.TileSize / (m_GhostSpeed * frameDuration)) * frameDuration;
        // also fractioned values would have to be stored, meaning that more than one "next coordinate" would have
        // to be calculated ahead of time to prevent movement stuttering... and probably other stuff, but I decided
        // not to think about it :)
        // **

        // initialize movement
        for (int i = 0; i < m_StartingGhostAmount; i++)
            SetGhostToRoaming(i);
        m_MovementStartTime = GameScreen.RoundDuration;
    }

    public static void UpdateBrain(Player player)
    {
        // move ghosts
        float elapsedTime = (float)(GameScreen.RoundDuration - m_MovementStartTime).TotalSeconds;
        float elapsedPercent = elapsedTime / m_MovementDuration;

        if (elapsedPercent >= 1)
        {
            for (int i = 0; i < m_Ghosts.Count; i++)
            {
                m_Ghosts[i].SetPosition(m_GhostPaths[i][1]);
                m_GhostPaths[i].RemoveAt(0);

                DetermineNextMovement(i, player);

                // set next direction
                Direction4 nextDirection = MapGrid.PositionToGridCoordinate(m_GhostPaths[i][1] - m_GhostPaths[i][0]).ToDirection4();
                m_Ghosts[i].SetDirection(nextDirection);
            }

            m_MovementStartTime = GameScreen.RoundDuration;
        }
        else
        {
            // move all ghosts
            for (int i = 0; i < m_Ghosts.Count; i++)
            {
                Vector2 newPosition = Vector2.Lerp(m_GhostPaths[i][0].ToVector2(), m_GhostPaths[i][1].ToVector2(), elapsedPercent);
                m_Ghosts[i].SetPosition(newPosition.ToPoint());
            }
        }
    }

    private static void DetermineNextMovement(int i, Player player)
    {
        // determine ghost behavior
        float squareDistance = Vector2.DistanceSquared(m_Ghosts[i].Position.ToVector2(), player.Position.ToVector2());
        float correctSquareRangeToCheckAgainst = m_GhostBehaviors[i] == GhostBehavior.Roaming ? m_GhostSquareRange_Roaming : m_GhostSquareRange_ChasingOrFleeing;
        bool isPlayerInRange = squareDistance <= correctSquareRangeToCheckAgainst;

        if (!GameScreen.HasRoundEnded && isPlayerInRange && player.State is PlayerState_Default)
        {
            SetGhostToChasing(i, player);
        }
        else if (!GameScreen.HasRoundEnded && isPlayerInRange && player.State is PlayerState_PoweredUp)
        {
            SetGhostToFleeing(i, player);
        }
        else  // if distance large enough or player is in untargetable state, make ghost roam
        {
            SetGhostToRoaming(i);
        }
    }

    private static void SetGhostToRoaming(int i)
    {
        bool needNewPath = m_GhostPaths[i].Count < 2;
        if (m_GhostBehaviors[i] != GhostBehavior.Roaming)
        {
            m_GhostBehaviors[i] = GhostBehavior.Roaming;
            m_Ghosts[i].Renderer.SetColor(Color.White);

            // needNewPath = true; // <-- no need to bother recalculating a new path when ghost restarts roaming
        }

        if (!needNewPath)
            return;

        Point currentCoord = MapGrid.PositionToGridCoordinate(m_Ghosts[i].Position);
        Direction4 currentDirection = m_Ghosts[i].CurrentDirection;

        MapGrid.GetRandomPath(currentCoord, currentDirection, m_RoamPathSize, out List<Point> newPath);

        m_GhostPaths[i].Clear();
        foreach (var coord in newPath)
            m_GhostPaths[i].Add(MapGrid.GridCoordinateToPosition(coord));
    }

    private static void SetGhostToChasing(int i, Player player)
    {
        bool needNewPath = m_GhostPaths[i].Count < 2;
        if (m_GhostBehaviors[i] != GhostBehavior.Chasing)
        {
            m_GhostBehaviors[i] = GhostBehavior.Chasing;
            m_Ghosts[i].Renderer.SetColor(Color.Red);

            needNewPath = true;
        }

        if (!needNewPath)
            return;

        Point currentCoord = MapGrid.PositionToGridCoordinate(m_Ghosts[i].Position);
        Point playerCoord = MapGrid.PositionToGridCoordinate(player.CenterPosition);
        MapGrid.GetPathTo(currentCoord, playerCoord, m_ChasePathSize, out List<Point> newPath);

        m_GhostPaths[i].Clear();
        foreach (var coord in newPath)
            m_GhostPaths[i].Add(MapGrid.GridCoordinateToPosition(coord));
    }
    private static void SetGhostToFleeing(int i, Player player)
    {
        bool needNewPath = m_GhostPaths[i].Count < 2;
        if (m_GhostBehaviors[i] != GhostBehavior.Fleeing)
        {
            m_GhostBehaviors[i] = GhostBehavior.Fleeing;
            m_Ghosts[i].Renderer.SetColor(Color.RoyalBlue);

            needNewPath = true;
        }

        if (!needNewPath)
            return;

        Point currentCoord = MapGrid.PositionToGridCoordinate(m_Ghosts[i].Position);
        Point playerCoord = MapGrid.PositionToGridCoordinate(player.Position);
        MapGrid.GetPathAwayFrom(currentCoord, playerCoord, m_FleePathSize, out List<Point> newPath);

        m_GhostPaths[i].Clear();
        foreach (var coord in newPath)
            m_GhostPaths[i].Add(MapGrid.GridCoordinateToPosition(coord));
    }

    public static void DrawGhosts(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < m_Ghosts.Count; i++)
        {
            m_Ghosts[i].Draw(spriteBatch);
        }
    }
}