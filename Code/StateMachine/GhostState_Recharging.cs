using Microsoft.Xna.Framework;

namespace topdown1;

public class GhostState_Recharging : GhostState
{
    public override bool IsCollidable => false;
    private float m_Duration;
    private double m_StateStartTime;

    public GhostState_Recharging(Ghost ghost) : base(ghost)
    {

    }

    public override void Update(GameTime gameTime, float movePercent)
    {
        double stateElapsedTime = GameScreen.RoundDuration.TotalSeconds - m_StateStartTime;
        if (stateElapsedTime >= m_Duration)
        {
            Respawn();
        }
    }

    public override void Enter()
    {
        m_Ghost.SetVisibility(false);

        m_StateStartTime = GameScreen.RoundDuration.TotalSeconds;
        m_Duration = 7;
    }

    public override void Exit()
    {
        m_Ghost.SetVisibility(true);
    }

    public override void Die()
    {
        // do nothing. this shouldn't be called anymore because of IsCollidable, but just in case...
    }

    private void Respawn()
    {
        m_Ghost.SetState(new GhostState_Roaming(m_Ghost));
    }
}