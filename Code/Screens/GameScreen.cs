using System;
using System.Collections.Generic;
using LDtk;
using LDtk.ContentPipeline;
using LDtk.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace topdown1;

public class GameScreen : AbstractScreen
{
    private LDtkWorld m_World;
    private List<LDtkLevel> m_LevelList;

    private Player m_Player;

    public static TimeSpan RoundDuration { get; private set; }
    public bool m_IsPaused;

    public override void Load()
    {
        LDtkFile file = LDtkFile.FromFile("Content/map/level-layout.ldtk");
        m_World = file.LoadWorld(new Guid("9b923070-3b70-11ee-9cfe-037a95b3620d"));

        m_LevelList = new List<LDtkLevel>(m_World.Levels);
        MapGrid.Initialize(m_LevelList[0], new LDtkRenderer(ScreenManager.SharedSpriteBatch));

        // create player
        var playerEntity = m_World.GetEntity<PlayerEntity>();
        m_Player = new Player(playerEntity);

        // initialize enemies
        GhostManager.Initialize();

        // set start time
        RoundDuration = TimeSpan.Zero;
    }

    public override bool HandleInput(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (InputState.GetPressed(InputCommands.UI_SUBMIT))
        {
            PauseGame();
        }

        // move player
        Vector2 resultMovement = Vector2.Zero;
        if (InputState.GetPressing(InputCommands.LEFT))
        {
            resultMovement += Vector2.UnitX * -1;
        }
        else if (InputState.GetPressing(InputCommands.RIGHT))
        {
            resultMovement += Vector2.UnitX;
        }

        if (InputState.GetPressing(InputCommands.UP))
        {
            resultMovement += Vector2.UnitY * -1;
        }
        else if (InputState.GetPressing(InputCommands.DOWN))
        {
            resultMovement += Vector2.UnitY;
        }

        m_Player.Move(resultMovement, gameTime.ElapsedGameTime.TotalSeconds);

        return true;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        if (m_IsPaused)
            return;

        RoundDuration += gameTime.ElapsedGameTime;

        GhostManager.UpdateBrain(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        MapGrid.Draw(spriteBatch);

        GhostManager.DrawGhosts(spriteBatch);

        m_Player.Draw(spriteBatch);
    }

    private void GoToStartScene()
    {
        Console.WriteLine("Going to Start scene...");

        ScreenManager.RemoveAllScreens();
        ScreenManager.AddScreen(new StartScreen());
    }

    private void PauseGame()
    {
        m_IsPaused = true;

        PauseScreen pauseScreen = new PauseScreen();
        pauseScreen.EventUnpaused += OnGameUnpaused;
        ScreenManager.AddScreen(pauseScreen);
    }

    private void OnGameUnpaused()
    {
        m_IsPaused = false;
    }
}