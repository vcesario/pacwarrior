using Microsoft.Xna.Framework;

namespace topdown1;

public class PlayerState_Spawning : PlayerState
{
    public PlayerState_Spawning(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        m_Player.ReturnToStartPosition();

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
    }
}