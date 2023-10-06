using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class GameOverScreen : AbstractScreen
{
    private SpriteFontBase m_TitleFont;
    private Vector2 m_TitleScreenPosition, m_TitlePivot;
    private string m_Title = "GAME OVER";

    private SpriteFontBase m_CaptionFont;
    private string m_Caption = "Press space to continue";
    private Vector2 m_CaptionScreenPosition;
    private Vector2 m_CaptionPivot;

    public GameOverScreen(bool playerWon)
    {
        m_Title = playerWon ? "YOU WON!" : "GAME OVER";
    }

    public override void Load()
    {
        m_TitleFont = GameText.Font_OpenSansBold.GetFont(32);
        m_TitleScreenPosition = new Vector2(GameStartup.GameWindow.ClientBounds.Width / 2f, GameStartup.GameWindow.ClientBounds.Height / 2.1f);
        m_TitlePivot = m_TitleFont.MeasureString(m_Title) * .5f;

        m_CaptionFont = GameText.Font_OpenSans.GetFont(26);
        m_CaptionScreenPosition = new Vector2(GameStartup.GameWindow.ClientBounds.Width / 2f, GameStartup.GameWindow.ClientBounds.Height / 1.25f);
        m_CaptionPivot = m_CaptionFont.MeasureString(m_Caption) / 2f;
    }

    public override bool HandleInput(GameTime gameTime)
    {
        if (InputState.GetPressed(InputCommands.UI_SUBMIT))
        {
            LoadStartScreen();
        }

        return true;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        ScreenManager.FadeBackBufferToBlack(.85f);
        spriteBatch.DrawString(m_TitleFont, m_Title, m_TitleScreenPosition, Color.White, origin: m_TitlePivot, effect: FontSystemEffect.Stroked, effectAmount: 2);

        spriteBatch.DrawString(m_CaptionFont, m_Caption, m_CaptionScreenPosition, Color.White, origin: m_CaptionPivot/*, scale: Vector2.One, effect: FontSystemEffect.Stroked, effectAmount: 3*/);
    }

    private void LoadStartScreen()
    {
        ScreenManager.RemoveAllScreens();
        ScreenManager.AddScreen(new StartScreen());
    }
}