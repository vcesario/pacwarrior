using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LDtk;
using LDtk.Renderer;

namespace topdown1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private LDtkWorld m_World;
    private LDtkRenderer m_Renderer;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);

        // screen size
        _graphics.PreferredBackBufferWidth = 3 * 16 * 16;
        _graphics.PreferredBackBufferHeight = 3 * 9 * 16;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        LDtkFile file = LDtkFile.FromFile("Content/map/level-layout.ldtk");
        m_World = file.LoadWorld(new System.Guid("9b923070-3b70-11ee-9cfe-037a95b3620d"));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // load game content here
        m_Renderer = new LDtkRenderer(_spriteBatch);
        foreach (var level in m_World.Levels)
            m_Renderer.PrerenderLevel(level);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // update logic here
        // **

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // drawing code here
        _spriteBatch.Begin();
        foreach (var level in m_World.Levels)
        {
            m_Renderer.RenderPrerenderedLevel(level);
        }
        _spriteBatch.End();
        // **

        base.Draw(gameTime);
    }
}
