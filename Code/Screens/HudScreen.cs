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
    private int m_CurrentLives;
    private string m_CurrentScoreString;

    public override void Load()
    {
        m_HudInfoFont = GameText.Font_OpenSans.GetFont(18);
        m_HudInfoScreenPosition = new Vector2(10, 10);

        UpdateLives();
        UpdateScore();
    }

    public override bool HandleInput(GameTime gameTime)
    {
        return false;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        m_HudInfo = "LIVES: ";
        for (int i = 0; i < m_CurrentLives; i++)
            m_HudInfo += "[ * ]";

        m_HudInfo += $"\nSCORE: {m_CurrentScoreString}";

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

    public override void ReceiveMessage(GameMessages message)
    {
        switch (message)
        {
            case GameMessages.PlayerLivesChanged:
                UpdateLives();
                break;
            case GameMessages.PlayerScoreChanged:
                UpdateScore();
                break;
            default:
                base.ReceiveMessage(message);
                break;
        }
    }

    private void UpdateLives()
    {
        GameScreen gameScreen = ScreenManager.GetScreen<GameScreen>();
        if (gameScreen == null)
            return;

        m_CurrentLives = gameScreen.GetRemainingLives();
    }

    private void UpdateScore()
    {
        GameScreen gameScreen = ScreenManager.GetScreen<GameScreen>();
        if (gameScreen == null)
            return;

        m_CurrentScoreString = gameScreen.GetScore().ToString("D3");
    }
}