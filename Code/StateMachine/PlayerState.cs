using System;
using Microsoft.Xna.Framework;

namespace topdown1;

public abstract class PlayerState : IState
{
    protected Player m_Player;

    public PlayerState(Player player)
    {
        m_Player = player;
    }

    public abstract void ProcessInput(GameTime gameTime);
    public abstract void Update(GameTime gameTime);
    public virtual void ReceiveMessage(StateMessages message)
    {
        if (GameStartup.DebugEnabled)
        {
            Console.WriteLine($"Current state \"{GetType()}\" can't interpret message \"{message}\".");
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
}