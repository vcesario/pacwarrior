using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class HudScreen : AbstractScreen
{
    private Texture2D m_DebugTexture;

    public override void Load()
    {
        m_DebugTexture = new Texture2D(ScreenManager.SharedGraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        m_DebugTexture.SetData(new[] { Color.CadetBlue });
    }

    public override bool HandleInput(GameTime gameTime, InputState input)
    {
        return false;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(m_DebugTexture, new Rectangle(0, 0, 100, 100), Color.White);
    }
}