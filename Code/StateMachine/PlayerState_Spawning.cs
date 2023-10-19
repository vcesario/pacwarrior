using System;
using Microsoft.Xna.Framework;

namespace topdown1;

public class PlayerState_Spawning : PlayerState
{
    protected override bool CanMove => true;

    protected override bool CanCollectThings => true;

    protected override bool CanCollideWithGhosts => false;

    protected override bool CanPause => true;

    private float m_Duration;
    private TimeSpan m_StartTime;

    public PlayerState_Spawning(Player player) : base(player)
    {
        m_Duration = 3;
    }

    public override void Enter()
    {
        m_Player.ReturnToStartPosition();

        m_StartTime = GameScreen.RoundDuration;

        base.Enter();
    }

    public override void Update(GameTime gameTime)
    {
        double elapsed = (GameScreen.RoundDuration - m_StartTime).TotalSeconds;
        double visibilityFactor = Math.Sin(elapsed * 15);
        m_Player.SetInvisible(visibilityFactor > 0);

        if (elapsed < m_Duration)
        {
            base.Update(gameTime);
        }
        else
        {
            GoToDefault();
        }
    }

    public override void Exit()
    {
        m_Player.SetInvisible(false);

        base.Exit();
    }

    public void GoToDefault()
    {
        m_Player.SetState(new PlayerState_Default(m_Player));
    }
}