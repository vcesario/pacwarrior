using System;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace topdown1;

public class StartScreen : AbstractScreen
{
    private SpriteFontBase m_TitleFont;
    private string m_TitleLine1 = "PAC";
    private string m_TitleLine2 = "WARRIOR";
    private Vector2 m_TitleScreenPosition;
    private Vector2 m_TitleLine1Pivot;
    private Vector2 m_TitleLine2Pivot;

    private SpriteFontBase m_CaptionFont;
    private string m_Caption = "Press space to play";
    private Vector2 m_CaptionScreenPosition;
    private Vector2 m_CaptionPivot;

    public override void Load()
    {
        // title
        m_TitleFont = GameText.Font_OpenSansBold.GetFont(72);
        m_TitleScreenPosition = new Vector2(GameStartup.GameWindow.ClientBounds.Width / 2f, GameStartup.GameWindow.ClientBounds.Height / 3f);
        m_TitleLine1Pivot = m_TitleFont.MeasureString(m_TitleLine1) / 2f;
        m_TitleLine2Pivot = m_TitleFont.MeasureString(m_TitleLine2) / 2f;

        // caption
        m_CaptionFont = GameText.Font_OpenSans.GetFont(26);
        m_CaptionScreenPosition = new Vector2(GameStartup.GameWindow.ClientBounds.Width / 2f, GameStartup.GameWindow.ClientBounds.Height / 1.25f);
        m_CaptionPivot = m_CaptionFont.MeasureString(m_Caption) / 2f;
    }

    public override bool HandleInput(GameTime gameTime)
    {
        if (InputState.GetPressed(InputCommands.UI_SUBMIT))
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

        spriteBatch.DrawString(m_TitleFont, m_TitleLine1, m_TitleScreenPosition + Vector2.UnitY * -1 * 20, Color.Goldenrod, origin: m_TitleLine1Pivot, scale: Vector2.One, effect: FontSystemEffect.Stroked, effectAmount: 3);
        spriteBatch.DrawString(m_TitleFont, m_TitleLine2, m_TitleScreenPosition + Vector2.UnitY * 20, Color.Goldenrod, origin: m_TitleLine2Pivot, scale: Vector2.One, effect: FontSystemEffect.Stroked, effectAmount: 3);

        spriteBatch.DrawString(m_CaptionFont, m_Caption, m_CaptionScreenPosition, Color.White, origin: m_CaptionPivot/*, scale: Vector2.One, effect: FontSystemEffect.Stroked, effectAmount: 3*/);
    }

    private void LoadGameplay()
    {
        Console.WriteLine("Going to Gameplay scene...");

        ScreenManager.RemoveAllScreens();
        ScreenManager.AddScreen(new GameScreen(), new RoundIntroScreen());
    }
}