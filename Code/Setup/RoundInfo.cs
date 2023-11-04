using System.Collections;
using System.Collections.Generic;

namespace topdown1;

public static class RoundInfo
{
    // @TODO: move over useful round information to this static class so all code can have access to. such as:
    public static List<Player> m_Players;
    public static IEnumerable<Player> Players => m_Players;

    public static void SetPlayers(params Player[] players)
    {
        if (m_Players == null)
            m_Players = new List<Player>();
        else
            m_Players.Clear();

        m_Players.AddRange(players);
    }

    public static Player GetPlayer(int index)
    {
        if (m_Players == null || index >= m_Players.Count || index < 0)
            return null;

        return m_Players[index];
    }

    // TimeSpan RoundStarted
    // bool IsPaused
    // bool HasEnded
}