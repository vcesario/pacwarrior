using System;
using System.Collections.Generic;
using LDtk;
using LDtk.ContentPipeline;
using LDtk.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using topdown1;

public class GameScreen : AbstractScreen
{
    private LDtkWorld m_World;
    private List<LDtkLevel> m_LevelList;
    private SpriteBatch m_SpriteBatch;

    private Player m_Player;

    public GameScreen(ScreenManager screenManager, SpriteBatch spriteBatch) : base(screenManager)
    {
        m_SpriteBatch = spriteBatch;
    }

    public override void Load()
    {
        LDtkFile file = LDtkFile.FromFile("Content/map/level-layout.ldtk");
        m_World = file.LoadWorld(new Guid("9b923070-3b70-11ee-9cfe-037a95b3620d"));

        m_LevelList = new List<LDtkLevel>(m_World.Levels);
        MapGrid.Initialize(m_LevelList[0], new LDtkRenderer(m_SpriteBatch));

        var playerEntity = m_World.GetEntity<PlayerEntity>();
        m_Player = new Player(playerEntity);
    }

    public override void HandleInput(GameTime gameTime, InputState input)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Enter))
        {
            GoToStartScene();
        }

        // move player
        if (keyboardState.IsKeyDown(Keys.A))
        {
            m_Player.Move(Vector2.UnitX * -1, gameTime.ElapsedGameTime.TotalSeconds);
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            m_Player.Move(Vector2.UnitX, gameTime.ElapsedGameTime.TotalSeconds);
        }
        else if (keyboardState.IsKeyDown(Keys.W))
        {
            m_Player.Move(Vector2.UnitY * -1, gameTime.ElapsedGameTime.TotalSeconds);
        }
        else if (keyboardState.IsKeyDown(Keys.S))
        {
            m_Player.Move(Vector2.UnitY, gameTime.ElapsedGameTime.TotalSeconds);
        }
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        MapGrid.Draw(spriteBatch);

        m_Player.Draw(spriteBatch);
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