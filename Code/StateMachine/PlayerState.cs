using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace topdown1;

public abstract class PlayerState : IState
{
    protected Player m_Player;

    protected abstract bool CanMove { get; }
    protected abstract bool CanCollectThings { get; }
    protected abstract bool CanCollideWithGhosts { get; }
    protected abstract bool CanPause { get; }

    private static List<Ghost> m_CollidedGhosts = new List<Ghost>();

    public PlayerState(Player player)
    {
        m_Player = player;
    }

    public virtual void ProcessInput(GameTime gameTime)
    {
        if (CanPause)
        {
            if (InputState.GetPressed(InputCommands.UI_SUBMIT))
            {
                PauseGame();
            }
        }
        if (CanMove)
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
    }

    public virtual void Update(GameTime gameTime)
    {
        BoundingBox playerBox = m_Player.GetColliderBox();

        if (CanCollectThings)
        {
            Collectible collected = null;
            foreach (var collectible in CollectibleManager.Collectibles)
            {
                if (playerBox.IsOverlapping(collectible.GetColliderBox()))
                {
                    collected = collectible;
                    break;
                }
            }

            collected?.Collect(m_Player);
        }

        if (GameScreen.HasRoundEnded)
            return;

        if (CanCollideWithGhosts)
        {
            foreach (var ghost in GhostManager.Ghosts)
            {
                if (ghost.State.IsCollidable && playerBox.IsOverlapping(ghost.GetColliderBox()))
                {
                    m_CollidedGhosts.Add(ghost);
                }
            }

            if (m_CollidedGhosts.Count > 0)
            {
                foreach (var ghost in m_CollidedGhosts)
                    ProcessGhostCollision(ghost);

                m_CollidedGhosts.Clear();
            }
        }
    }

    public virtual void Enter()
    {
        if (GameStartup.DebugEnabled)
        {
            Console.WriteLine($"Entering state = {GetType()}");
        }
    }
    public virtual void Exit()
    {
        if (GameStartup.DebugEnabled)
        {
            Console.WriteLine($"Exiting state = {GetType()}");
        }
    }

    protected virtual void ProcessGhostCollision(Ghost ghost)
    {
        m_Player.SetState(new PlayerState_Dying(m_Player));
    }

    public virtual void ProcessPowerUpCollection()
    {
        m_Player.SetState(new PlayerState_PoweredUp(m_Player));
    }

    private void PauseGame()
    {
        ScreenManager.SendMessageToScreens(GameMessages.PlayerPaused);
    }
}