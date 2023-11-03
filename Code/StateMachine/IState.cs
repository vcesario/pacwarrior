using Microsoft.Xna.Framework;

namespace topdown1;

public interface IState
{
    public void Update(GameTime gameTime);
    public void Enter();
    public void Exit();
}