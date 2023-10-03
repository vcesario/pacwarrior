using System;
using Microsoft.Xna.Framework;

namespace topdown1;

public class PlayerState_Default : PlayerState
{
    public PlayerState_Default(Player player) : base(player)
    {
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
        BoundingBox playerBox = m_Player.GetColliderBox();
        foreach (var ghost in GhostManager.Ghosts)
        {
            if (playerBox.IsOverlapping(ghost.GetColliderBox()))
            {
                // eliminate player
                ReceiveMessage(StateMessages.KillPlayer);
            }
        }
    }

    public override void ReceiveMessage(StateMessages message)
    {
        switch (message)
        {
            case StateMessages.KillPlayer:
                m_Player.SetState(new PlayerState_Dying(m_Player));
                break;
            default:
                base.ReceiveMessage(message);
                break;
        }
    }
}
