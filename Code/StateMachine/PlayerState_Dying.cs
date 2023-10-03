using System;
using Microsoft.Xna.Framework;

namespace topdown1;

public class PlayerState_Dying : PlayerState
{
    private TimeSpan m_DeathTime;
    private float m_DyingAnimationDuration;

    public PlayerState_Dying(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        m_DeathTime = GameScreen.RoundDuration;
        m_DyingAnimationDuration = 3;

        base.Enter();
    }

    public override void ProcessInput(GameTime gameTime)
    {
    }

    public override void Update(GameTime gameTime)
    {
        float elapsed = (float)(GameScreen.RoundDuration - m_DeathTime).TotalSeconds;
        if (elapsed >= m_DyingAnimationDuration)
        {
            ReceiveMessage(StateMessages.RespawnPlayer);
        }
        Console.WriteLine($"PLayer dead ({elapsed})");
    }

    public override void ReceiveMessage(StateMessages message)
    {
        switch (message)
        {
            case StateMessages.RespawnPlayer:
                m_Player.SetState(new PlayerState_Spawning(m_Player));
                break;
            default:
                base.ReceiveMessage(message);
                break;
        }
    }
}