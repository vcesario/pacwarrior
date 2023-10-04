using System;
using Microsoft.Xna.Framework;

namespace topdown1;

public class PlayerState_Dying : PlayerState
{
    private TimeSpan m_DeathTime;
    private float m_DyingAnimationDuration;
    private bool m_DeathConsequencesDetermined;

    public PlayerState_Dying(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        m_DeathTime = GameScreen.RoundDuration;
        m_DyingAnimationDuration = 3;

        m_Player.SetColor(Color.OrangeRed);

        base.Enter();
    }

    public override void ProcessInput(GameTime gameTime)
    {
    }

    public override void Update(GameTime gameTime)
    {
        float elapsed = (float)(GameScreen.RoundDuration - m_DeathTime).TotalSeconds;
        if (!m_DeathConsequencesDetermined && elapsed >= m_DyingAnimationDuration)
        {
            DetermineDeathConsequences();
            m_DeathConsequencesDetermined = true;
        }
    }

    /// <summary>
    /// Basically checks if player can play more, or the game has ended
    /// </summary>
    private void DetermineDeathConsequences()
    {
        m_Player.LoseLife();
        if (m_Player.LivesRemaining > 0)
        {
            GoToRespawn();
        }
        else
        {
            LoseGame();
        }
    }

    public override void Exit()
    {
        m_Player.SetColor(Color.White);

        base.Exit();
    }

    private void GoToRespawn()
    {
        m_Player.SetState(new PlayerState_Spawning(m_Player));
    }

    private void LoseGame()
    {
        ScreenManager.SendMessageToScreens(GameMessages.PlayerLostGame);
    }
}