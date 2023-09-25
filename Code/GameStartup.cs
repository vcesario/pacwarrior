using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LDtk;
using LDtk.Renderer;
using Microsoft.Xna.Framework.Content;

namespace topdown1;

public class GameStartup : Game
{
    private static GameStartup m_StaticReference;

    public GraphicsDeviceManager Graphics
    { get; private set; }

    private ScreenManager m_ScreenManager;

    public static ContentManager ContentManager => m_StaticReference.Content;

    public GameStartup()
    {
        m_StaticReference = this;

        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        
        m_ScreenManager = new ScreenManager(this);
        Components.Add(m_ScreenManager);
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
