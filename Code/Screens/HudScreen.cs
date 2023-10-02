using System;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class HudScreen : AbstractScreen
{
    private SpriteFontBase m_HudInfoFont;
    private Vector2 m_HudInfoScreenPosition;
    private string m_HudInfo;

    public override void Load()
    {
        m_HudInfoFont = GameText.Font_OpenSans.GetFont(18);
        m_HudInfoScreenPosition = new Vector2(10, 10);
    }

    public override bool HandleInput(GameTime gameTime)
    {
        return false;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        m_HudInfo = "LIVES: 0";

        m_HudInfo += "\nSCORE: 000";

        m_HudInfo += $"\nTIME: {GameScreen.RoundDuration.ToString(@"mm\:ss")}";
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (GameStartup.DebugEnabled)
        {
            FPSCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            m_HudInfo += $"\n\nFPS: {FPSCounter.AverageFramesPerSecond}s";
        }

        spriteBatch.DrawString(m_HudInfoFont, m_HudInfo, m_HudInfoScreenPosition, Color.White);
    }
}