using System;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class HudScreen : AbstractScreen
{
    private SpriteFontBase m_HudInfoSpriteFont;
    private Vector2 m_HudInfoPosition;
    private string m_HudInfoText;

    public override void Load()
    {
        m_HudInfoSpriteFont = GameText.Font_OpenSans.GetFont(18);
        m_HudInfoPosition = new Vector2(10, 10);
    }

    public override bool HandleInput(GameTime gameTime, InputState input)
    {
        return false;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        m_HudInfoText = "LIVES: 0";

        m_HudInfoText += "\nSCORE: 000";

        TimeSpan matchLength = DateTime.Now - GameScreen.RoundStartTime;
        m_HudInfoText += $"\nTIME: {matchLength.ToString(@"mm\:ss")}";
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (GameStartup.DebugEnabled)
        {
            FPSCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            m_HudInfoText += $"\n\nFPS: {FPSCounter.AverageFramesPerSecond}s";
        }

        spriteBatch.DrawString(m_HudInfoSpriteFont, m_HudInfoText, m_HudInfoPosition, Color.White);
    }
}