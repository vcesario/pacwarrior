using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public abstract class AbstractScreen
{
    public ScreenStates ScreenState;

    public bool IsPopup
    { get; }

    public abstract void Load();

    /// <summary>
    /// Pretty self explanatory.
    /// </summary>
    /// <returns><c>True</c> if I want this screen to prevent input from passing through. <c>False</c> if I don't.</returns>
    public abstract bool HandleInput(GameTime gameTime);
    public abstract void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen);

    /// <summary>
    /// Reminder: Sprite batch already begins/ends outside of this.
    /// </summary>
    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    public virtual void ReceiveMessage(GameMessages message)
    {

    }
}