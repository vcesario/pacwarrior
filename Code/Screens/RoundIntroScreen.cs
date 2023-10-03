using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class RoundIntroScreen : AbstractScreen
{
    public event Action EventClosed;
    private float m_Elapsed;
    private float m_IntroDuration;

    public override void Load()
    {
        m_Elapsed = 0;
        m_IntroDuration = 3;
    }

    public override bool HandleInput(GameTime gameTime)
    {
        return true;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        // update animation
        // on animation ended, EndIntro()

        m_Elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (m_Elapsed >= m_IntroDuration)
            EndIntro();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // get current frame from animation

        float alpha = MathF.Max(0, 1 - m_Elapsed / m_IntroDuration);
        ScreenManager.FadeBackBufferToBlack(alpha);
    }

    private void EndIntro()
    {
        // remove screen from manager
        ScreenManager.RemoveScreen<RoundIntroScreen>();
        EventClosed?.Invoke();
    }
}