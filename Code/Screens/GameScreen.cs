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
        // set start time
        RoundDuration = TimeSpan.Zero;

        LDtkFile file = LDtkFile.FromFile("Content/map/level-layout.ldtk");
        m_World = file.LoadWorld(new Guid("9b923070-3b70-11ee-9cfe-037a95b3620d"));

        m_LevelList = new List<LDtkLevel>(m_World.Levels);
        MapGrid.Initialize(m_LevelList[0], new LDtkRenderer(ScreenManager.SharedSpriteBatch));

        // create player
        var playerEntity = m_World.GetEntity<PlayerEntity>();
        m_Player = new Player(playerEntity);

        // initialize enemies
        GhostManager.Initialize();

        // if there's an intro, initial pause to wait for intro to end
        if (ScreenManager.IsScreenOpen<RoundIntroScreen>())
        {
            m_IsPaused = true;
            ScreenManager.GetScreen<RoundIntroScreen>().EventClosed += StartGame;
        }
    }

    public override bool HandleInput(GameTime gameTime)
    {
        if (InputState.GetPressed(InputCommands.UI_SUBMIT))
        {
            PauseGame();
        }

        m_Player.State.ProcessInput(gameTime);

        return true;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        if (m_IsPaused)
            return;

        RoundDuration += gameTime.ElapsedGameTime;

        GhostManager.UpdateBrain();

        m_Player.State.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        MapGrid.Draw(spriteBatch);

        GhostManager.DrawGhosts(spriteBatch);

        m_Player.Draw(spriteBatch);
    }

    private void StartGame()
    {
        UnpauseGame();
        ScreenManager.AddScreen(new HudScreen());

        Console.WriteLine("Game started!");

        // instantiate coins
        // set player state to playing
    }

    private void PauseGame()
    {
        m_IsPaused = true;

        PauseScreen pauseScreen = new PauseScreen();
        pauseScreen.EventUnpaused += UnpauseGame;
        ScreenManager.AddScreen(pauseScreen);
    }

    private void UnpauseGame()
    {
        m_IsPaused = false;
    }

    public void EndGame()
    {

    }

    public int GetRemainingLives()
    {
        if (m_Player == null)
            return 0;

        return m_Player.LivesRemaining;
    }

    public override void ReceiveMessage(GameMessages message)
    {
        switch (message)
        {
            case GameMessages.PlayerLostGame:
                ScreenManager.RemoveScreen<HudScreen>();
                ScreenManager.AddScreen(new GameOverScreen());
                break;
            default:
                base.ReceiveMessage(message);
                break;
        }
    }
}