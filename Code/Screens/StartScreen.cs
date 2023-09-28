using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace topdown1;

public class StartScreen : AbstractScreen
{
    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
    }

    public override bool HandleInput(GameTime gameTime, InputState input)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            LoadGameplay();
        }

        return true;
    }

    public override void Load()
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.GraphicsDevice.Clear(Color.RoyalBlue);
    }

    private void LoadGameplay()
    {
        Console.WriteLine("Going to Gameplay scene...");

        ScreenManager.RemoveAllScreens();
        ScreenManager.AddScreen(new GameScreen(ScreenManager.SharedSpriteBatch), new HudScreen());
    }
}