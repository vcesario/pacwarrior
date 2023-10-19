using System;
using Microsoft.Xna.Framework;

namespace topdown1;

public class PlayerState_PoweredUp : PlayerState
{
    private float m_Duration;
    private TimeSpan m_StartTime;

    public PlayerState_PoweredUp(Player player) : base(player)
    {
        m_Duration = 5;
    }

    protected override bool CanMove => true;

    protected override bool CanCollectThings => true;

    protected override bool CanCollideWithGhosts => true;

    protected override bool CanPause => true;

    public override void Enter()
    {
        m_Player.Renderer.SetColor(Color.DarkRed);
        m_StartTime = GameScreen.RoundDuration;

        base.Enter();
    }

    public override void Update(GameTime gameTime)
    {
        float elapsed = (float)(GameScreen.RoundDuration - m_StartTime).TotalSeconds;
        if (elapsed >= m_Duration)
        {
            TerminatePowerUp();
        }

        base.Update(gameTime);
    }

    public override void Exit()
    {
        m_Player.Renderer.SetColor(Color.White);
        base.Exit();
    }


    protected override void ProcessGhostCollision(Ghost ghost)
    {
        // kill ghost
    }

    public override void ProcessPowerUpCollection()
    {
        // do nothing
    }

    private void TerminatePowerUp()
    {
        m_Player.SetState(new PlayerState_Default(m_Player));
    }
}