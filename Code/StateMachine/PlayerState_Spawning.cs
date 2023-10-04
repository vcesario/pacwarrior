using System;
using Microsoft.Xna.Framework;

namespace topdown1;

public class PlayerState_Spawning : PlayerState
{
    private float m_Duration;
    private TimeSpan m_StartTime;

    public PlayerState_Spawning(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        m_Player.ReturnToStartPosition();

        m_Duration = 3;
        m_StartTime = GameScreen.RoundDuration;

        base.Enter();
    }

    public override void ProcessInput(GameTime gameTime)
    {
        // move player
        Vector2 resultMovement = Vector2.Zero;
        if (InputState.GetPressing(InputCommands.LEFT))
        {
            resultMovement += Vector2.UnitX * -1;
        }
        else if (InputState.GetPressing(InputCommands.RIGHT))
        {
            resultMovement += Vector2.UnitX;
        }

        if (InputState.GetPressing(InputCommands.UP))
        {
            resultMovement += Vector2.UnitY * -1;
        }
        else if (InputState.GetPressing(InputCommands.DOWN))
        {
            resultMovement += Vector2.UnitY;
        }

        m_Player.Move(resultMovement, gameTime.ElapsedGameTime.TotalSeconds);
    }

    public override void Update(GameTime gameTime)
    {
        double elapsed = (GameScreen.RoundDuration - m_StartTime).TotalSeconds;
        double visibilityFactor = Math.Sin(elapsed * 15);
        m_Player.SetInvisible(visibilityFactor > 0);

        if (elapsed >= m_Duration)
        {
            ReceiveMessage(GameMessages.EndPlayerSpawning);
        }
    }

    public override void ReceiveMessage(GameMessages message)
    {
        switch (message)
        {
            case GameMessages.EndPlayerSpawning:
                m_Player.SetState(new PlayerState_Default(m_Player));
                break;
            default:
                base.ReceiveMessage(message);
                break;
        }
    }

    public override void Exit()
    {
        m_Player.SetInvisible(false);

        base.Exit();
    }
}