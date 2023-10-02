using System;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace topdown1;

public class PauseScreen : AbstractScreen
{
    private SpriteFontBase m_CaptionFont;
    private Vector2 m_CaptionScreenPosition, m_CaptionPivot;
    private string m_Caption = "PAUSE";

    public event Action EventUnpaused;

    public override void Load()
    {
        m_CaptionFont = GameText.Font_OpenSansBold.GetFont(32);
        m_CaptionScreenPosition = new Vector2(GameStartup.GameWindow.ClientBounds.Width / 2f, GameStartup.GameWindow.ClientBounds.Height / 2.1f);
        m_CaptionPivot = m_CaptionFont.MeasureString(m_Caption) * .5f;
    }

    public override bool HandleInput(GameTime gameTime)
    {
        if (InputState.GetPressed(InputCommands.UI_SUBMIT))
        {
            Unpause();
        }

        return true;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        ScreenManager.FadeBackBufferToBlack(.65f);

        spriteBatch.DrawString(m_CaptionFont, m_Caption, m_CaptionScreenPosition, Color.White, origin: m_CaptionPivot, effect: FontSystemEffect.Stroked, effectAmount: 2);
    }

    private void Unpause()
    {
        ScreenManager.RemoveScreen(GetType());
        // @TODO: transition before invoke to prevent getting caught up on input loops
        EventUnpaused?.Invoke();
    }
}