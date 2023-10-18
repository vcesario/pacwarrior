using System;
using Microsoft.Xna.Framework;

namespace topdown1;

public abstract class PlayerState : IState
{
    protected Player m_Player;

    protected abstract bool CanMove { get; }
    protected abstract bool CanCollectThings { get; }
    protected abstract bool CanCollideWithGhosts { get; }
    protected abstract bool CanPause { get; }

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
            foreach (var collectible in CoinManager.Collectibles)
            {
                if (playerBox.IsOverlapping(collectible.GetColliderBox()))
                {
                    collected = collectible;
                }
            }
            CoinManager.Collect(collected);
        }

        if (GameScreen.HasRoundEnded)
            return;

        if (CanCollideWithGhosts)
        {
            foreach (var ghost in GhostAI.Ghosts)
            {
                if (playerBox.IsOverlapping(ghost.GetColliderBox()))
                {
                    // eliminate player
                    if (this is PlayerState_PoweredUp)
                    {
                        // kill ghost
                    }
                    else
                    {
                        KillPlayer();
                    }
                }
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

    private void KillPlayer()
    {
        m_Player.SetState(new PlayerState_Dying(m_Player));
    }

    private void PauseGame()
    {
        ScreenManager.SendMessageToScreens(GameMessages.PlayerPaused);
    }
}