using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LDtk;
using LDtk.Renderer;

namespace topdown1;

public class GameStartup : Game
{
    public GraphicsDeviceManager Graphics
    { get; private set; }

    private ScreenManager m_ScreenManager;

    public GameStartup()
    {
        Graphics = new GraphicsDeviceManager(this);

        m_ScreenManager = new ScreenManager(this);
        Components.Add(m_ScreenManager);

        Content.RootDirectory = "Content";
    }

    // protected override void Initialize()
    // {
    //     base.Initialize();
    // }

    // protected override void LoadContent()
    // {
    //     _spriteBatch = new SpriteBatch(GraphicsDevice);
    // }

    protected override void Update(GameTime gameTime)
    {
        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */
        Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // update logic here
        // **

        base.Update(gameTime);
    }

    // protected override void Draw(GameTime gameTime)
    // {
    //     GraphicsDevice.Clear(Color.CornflowerBlue);

    //     // drawing code here
    //     // **

    //     base.Draw(gameTime);
    // }
}
