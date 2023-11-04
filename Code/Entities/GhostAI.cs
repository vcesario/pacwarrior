using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class GhostAI
{
    private static List<Ghost> m_Ghosts;
    public static IEnumerable<Ghost> Ghosts => m_Ghosts;

    private static float m_GhostSpeed;
    private static float m_MovementDuration;
    private static TimeSpan m_MovementStartTime;

    private static int m_StartingGhostAmount;
    private static int m_KillScore;

    public static void Initialize()
    {
        // initialize balancing values
        m_GhostSpeed = 100;
        m_StartingGhostAmount = 1;
        m_KillScore = 5;

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
        for (int i = 0; i < m_StartingGhostAmount; i++)
        {
            Ghost newGhost = new Ghost(MapGrid.GridCoordinateToPosition(walkables[i]));

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
        m_MovementStartTime = GameScreen.RoundDuration;
    }

    public static void Update(GameTime gameTime)
    {
        // move ghosts
        float elapsedTime = (float)(GameScreen.RoundDuration - m_MovementStartTime).TotalSeconds;
        float elapsedPercent = elapsedTime / m_MovementDuration;

        for (int i = 0; i < m_Ghosts.Count; i++)
            m_Ghosts[i].State.Update(gameTime, elapsedPercent);

        if (elapsedPercent >= 1)
            m_MovementStartTime = GameScreen.RoundDuration;
    }


    public static void DrawGhosts(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < m_Ghosts.Count; i++)
        {
            m_Ghosts[i].Draw(spriteBatch);
        }
    }

    public static void Kill(Ghost ghost, Player player)
    {
        int index = m_Ghosts.IndexOf(ghost);
        m_Ghosts.Remove(ghost);

        player.AddScore(m_KillScore);
    }
}