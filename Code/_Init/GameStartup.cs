using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LDtk;
using LDtk.Renderer;
using Microsoft.Xna.Framework.Content;
using System;

namespace topdown1;

public class GameStartup : Game
{
    private static GameStartup m_StaticReference;

    public static GraphicsDeviceManager Graphics
    { get; private set; }

    private ScreenManager m_ScreenManager;

    public static ContentManager ContentManager => m_StaticReference.Content;
    public static GameWindow GameWindow => m_StaticReference.Window;

    private bool m_F1Pressed;
    public static bool DebugEnabled { get; private set; }

    public static Random RandomGenerator { get; private set; }


    public GameStartup()
    {
        m_StaticReference = this;

        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        m_ScreenManager = new ScreenManager(this);
        Components.Add(m_ScreenManager);

        RandomGenerator = new Random();
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
        KeyboardState keyboardState = Keyboard.GetState();
        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */
        keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        if (!m_F1Pressed && keyboardState.IsKeyDown(Keys.F1))
        {
            m_F1Pressed = true;

            DebugEnabled = !DebugEnabled;
            if (DebugEnabled)
                Console.WriteLine("Debug Enabled!");
            else
                Console.WriteLine("Debug disabled.");
        }
        if (m_F1Pressed && keyboardState.IsKeyUp(Keys.F1))
        {
            m_F1Pressed = false;
        }

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
