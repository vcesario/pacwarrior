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

        GameText.Initialize();
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
        InputState.Update(gameTime);

        if (InputState.GetPressed(InputCommands.UI_EXIT))
            Exit();

        if (InputState.GetPressed(InputCommands.DEBUG_1))
        {
            DebugEnabled = !DebugEnabled;
            if (DebugEnabled)
                Console.WriteLine("Debug enabled!");
            else
                Console.WriteLine("Debug disabled.");
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
