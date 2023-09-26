using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public abstract class AbstractScreen
{
    protected ScreenManager ScreenManager;
    public ScreenStates ScreenState;

    public bool IsPopup
    { get; }

    public AbstractScreen(ScreenManager screenManager)
    {
        ScreenManager = screenManager;
    }

    public abstract void Load();
    public abstract void HandleInput(GameTime gameTime, InputState input);
    public abstract void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen);
    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}