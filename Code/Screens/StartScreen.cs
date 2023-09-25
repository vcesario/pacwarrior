using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class StartScreen : AbstractScreen
{
    public StartScreen(ScreenManager screenManager) : base(screenManager)
    {
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
    }

    public override void HandleInput(GameTime gameTime, InputState input)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            GoToGameplayScene();
        }
    }

    public override void Load()
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.GraphicsDevice.Clear(Color.FloralWhite);
    }

    private void GoToGameplayScene()
    {
        Console.WriteLine("Going to Gameplay scene...");

        ScreenManager.RemoveScreenWithTransition(this, addGameScreen);

        void addGameScreen()
        {
            ScreenManager.AddScreenWithTransition(ScreenManager.InstantiateGameScreen());
        }
    }
}