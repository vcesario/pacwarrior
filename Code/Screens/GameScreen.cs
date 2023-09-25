using System;
using LDtk;
using LDtk.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameScreen : AbstractScreen
{
    private LDtkWorld m_World;
    private LDtkRenderer m_Renderer;
    private SpriteBatch m_SpriteBatch;

    public GameScreen(ScreenManager screenManager, SpriteBatch spriteBatch) : base(screenManager)
    {
        m_SpriteBatch = spriteBatch;
    }

    public override void HandleInput(GameTime gameTime, InputState input)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            GoToStartScene();
        }
    }

    public override void Load()
    {
        LDtkFile file = LDtkFile.FromFile("Content/map/level-layout.ldtk");
        m_World = file.LoadWorld(new System.Guid("9b923070-3b70-11ee-9cfe-037a95b3620d"));

        m_Renderer = new LDtkRenderer(m_SpriteBatch);
        foreach (var level in m_World.Levels)
            m_Renderer.PrerenderLevel(level);
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var level in m_World.Levels)
        {
            m_Renderer.RenderPrerenderedLevel(level);
        }
    }

    private void GoToStartScene()
    {
        Console.WriteLine("Going to Start scene...");

        ScreenManager.RemoveScreenWithTransition(this, addStartScreen);

        void addStartScreen()
        {
            ScreenManager.AddScreenWithTransition(ScreenManager.InstantiateStartScreen());
        }
    }
}