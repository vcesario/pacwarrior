using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public static class CoinManager
{
    private static List<Coin> m_Coins;

    public static void Initialize(Point playerPosition = default)
    {
        m_Coins = new List<Coin>();

        Point playerCoordinate = MapGrid.PositionToGridCoordinate(playerPosition);
        foreach (var coordinate in MapGrid.GetAllWalkableTiles())
        {
            if (playerCoordinate == coordinate)
                continue;

            m_Coins.Add(new Coin(MapGrid.GridCoordinateToPosition(coordinate)));
        }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (var coin in m_Coins)
            coin.Renderer.Draw(spriteBatch);
    }
}