using Microsoft.Xna.Framework;

namespace topdown1;

public class GhostState_Recharging : GhostState
{
    public override bool IsCollidable => false;

    public GhostState_Recharging(Ghost ghost) : base(ghost)
    {

    }

    public override void Update(GameTime gameTime, float movePercent)
    {
        // do nothing, for now
    }

    public override void Enter()
    {
        m_Ghost.SetVisibility(false);
    }

    public override void Exit()
    {
        m_Ghost.SetVisibility(true);
    }

    public override void Die()
    {
        // do nothing
    }
}