using System;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace topdown1;

public class StartScreen : AbstractScreen
{
    private FontSystem m_Font_OpenSansBold;
    private SpriteFontBase m_TitleSpriteFont;
    private string m_TitleLine1 = "PAC";
    private string m_TitleLine2 = "WARRIOR";
    private Vector2 m_TitleScreenPosition;
    private Vector2 m_TitleLine1Pivot;
    private Vector2 m_TitleLine2Pivot;

    private FontSystem m_Font_OpenSans;
    private SpriteFontBase m_CaptionSpriteFont;
    private string m_Caption = "Press space to play";
    private Vector2 m_CaptionScreenPosition;
    private Vector2 m_CaptionPivot;

    public override void Load()
    {
        // title
        m_Font_OpenSansBold = new FontSystem();
        m_Font_OpenSansBold.AddFont(File.ReadAllBytes("Assets/Fonts/OpenSans-Bold.ttf")); // @QUESTION: put '@' before string?
        m_TitleSpriteFont = m_Font_OpenSansBold.GetFont(72);
        m_TitleScreenPosition = new Vector2(GameStartup.GameWindow.ClientBounds.Width / 2f, GameStartup.GameWindow.ClientBounds.Height / 3f);
        m_TitleLine1Pivot = m_TitleSpriteFont.MeasureString(m_TitleLine1) / 2f;
        m_TitleLine2Pivot = m_TitleSpriteFont.MeasureString(m_TitleLine2) / 2f;

        // caption
        m_Font_OpenSans = new FontSystem();
        m_Font_OpenSans.AddFont(File.ReadAllBytes("Assets/Fonts/OpenSans-Regular.ttf"));
        m_CaptionSpriteFont = m_Font_OpenSans.GetFont(26);
        m_CaptionScreenPosition = new Vector2(GameStartup.GameWindow.ClientBounds.Width / 2f, GameStartup.GameWindow.ClientBounds.Height / 1.25f);
        m_CaptionPivot = m_CaptionSpriteFont.MeasureString(m_Caption) / 2f;
    }

    public override bool HandleInput(GameTime gameTime, InputState input)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            LoadGameplay();
        }

        return true;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.GraphicsDevice.Clear(Color.RoyalBlue);

        spriteBatch.DrawString(m_TitleSpriteFont, m_TitleLine1, m_TitleScreenPosition + Vector2.UnitY * -1 * 20, Color.Goldenrod, origin: m_TitleLine1Pivot, scale: Vector2.One, effect: FontSystemEffect.Stroked, effectAmount: 3);
        spriteBatch.DrawString(m_TitleSpriteFont, m_TitleLine2, m_TitleScreenPosition + Vector2.UnitY * 20, Color.Goldenrod, origin: m_TitleLine2Pivot, scale: Vector2.One, effect: FontSystemEffect.Stroked, effectAmount: 3);

        spriteBatch.DrawString(m_CaptionSpriteFont, m_Caption, m_CaptionScreenPosition, Color.White, origin: m_CaptionPivot/*, scale: Vector2.One, effect: FontSystemEffect.Stroked, effectAmount: 3*/);
    }

    private void LoadGameplay()
    {
        Console.WriteLine("Going to Gameplay scene...");

        ScreenManager.RemoveAllScreens();
        ScreenManager.AddScreen(new GameScreen(ScreenManager.SharedSpriteBatch), new HudScreen());
    }
}