using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class CollectibleManager
{
    public static IEnumerable<Collectible> Collectibles => m_Collectibles;
    private static List<Collectible> m_Collectibles;

    public static void Initialize(Point playerPosition, PowerUpEntity[] powerUpEntities)
    {
        m_Collectibles = new List<Collectible>();

        HashSet<Point> skippableCoordinates = new HashSet<Point>();
        foreach (var entity in powerUpEntities)
        {
            Collectible_PowerUp powerUp = new Collectible_PowerUp(entity.Position.ToPoint());
            powerUp.EventWasCollected += OnCollected;
            m_Collectibles.Add(powerUp);
            skippableCoordinates.Add(MapGrid.PositionToGridCoordinate(powerUp.Position));
        }

        skippableCoordinates.Add(MapGrid.PositionToGridCoordinate(playerPosition));
        foreach (var coordinate in MapGrid.WalkableCoordinates)
        {
            if (skippableCoordinates.Contains(coordinate))
                continue;

            Collectible_Coin coin = new Collectible_Coin(MapGrid.GridCoordinateToPosition(coordinate));
            coin.EventWasCollected += OnCollected;
            m_Collectibles.Add(coin);
        }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (var collectible in m_Collectibles)
            collectible.Renderer.Draw(spriteBatch);
    }

    public static void OnCollected(Collectible collectible)
    {
        m_Collectibles.Remove(collectible);

        if (m_Collectibles.Count == 0)
        {
            ScreenManager.SendMessageToScreens(GameMessages.PlayerWonRound);
        }
    }
}