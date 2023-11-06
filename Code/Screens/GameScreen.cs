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

    public static TimeSpan RoundDuration { get; private set; }
    public bool m_IsPaused;
    public static bool HasRoundEnded { get; private set; }

    public override void Load()
    {
        // set start time
        RoundDuration = TimeSpan.Zero;

        HasRoundEnded = false;

        LDtkFile file = LDtkFile.FromFile("Content/map/level-layout.ldtk");
        m_World = file.LoadWorld(new Guid("9b923070-3b70-11ee-9cfe-037a95b3620d"));

        m_LevelList = new List<LDtkLevel>(m_World.Levels);
        MapGrid.Initialize(m_LevelList[0], new LDtkRenderer(ScreenManager.SharedSpriteBatch));

        // create player
        var playerEntity = m_World.GetEntity<PlayerEntity>();
        RoundInfo.SetPlayers(new Player(playerEntity));

        // initialize enemies
        var enemySpawnerEntities = m_World.GetEntities<EnemySpawnerEntity>();
        GhostManager.Initialize(enemySpawnerEntities);

        // spawn coins
        var powerUpEntities = m_World.GetEntities<PowerUpEntity>();
        CollectibleManager.Initialize(RoundInfo.GetPlayer(0).Position, powerUpEntities);

        // if there's an intro, initial pause to wait for intro to end
        if (ScreenManager.IsScreenOpen<RoundIntroScreen>())
        {
            m_IsPaused = true;
            ScreenManager.GetScreen<RoundIntroScreen>().EventClosed += StartGame;
        }
    }

    public override bool HandleInput(GameTime gameTime)
    {
        RoundInfo.GetPlayer(0).State.ProcessInput(gameTime);

        return true;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        if (m_IsPaused)
            return;

        RoundDuration += gameTime.ElapsedGameTime;

        GhostManager.Update(gameTime);
        // foreach (var ghost in GhostAI.Ghosts)
        // {
        //     ghost.State.Update(gameTime);
        // }

        RoundInfo.GetPlayer(0).State.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        MapGrid.Draw(spriteBatch);

        CollectibleManager.Draw(spriteBatch);

        GhostManager.DrawGhosts(spriteBatch);

        RoundInfo.GetPlayer(0).Draw(spriteBatch);
    }

    private void StartGame()
    {
        UnpauseGame();
        ScreenManager.AddScreen(new HudScreen());

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
        if (RoundInfo.GetPlayer(0) == null)
            return 0;

        return RoundInfo.GetPlayer(0).LivesRemaining;
    }

    public int GetScore()
    {
        if (RoundInfo.GetPlayer(0) == null)
            return 0;

        return RoundInfo.GetPlayer(0).Score;
    }

    public override void ReceiveMessage(GameMessages message)
    {
        switch (message)
        {
            case GameMessages.PlayerLostRound:
                HasRoundEnded = true;
                ScreenManager.RemoveScreen<HudScreen>();
                ScreenManager.AddScreen(new GameOverScreen(false));
                break;

            case GameMessages.PlayerWonRound:
                HasRoundEnded = true;
                ScreenManager.RemoveScreen<HudScreen>();
                ScreenManager.AddScreen(new GameOverScreen(true));
                break;

            case GameMessages.PlayerPaused:
                PauseGame();
                break;

            default:
                base.ReceiveMessage(message);
                break;
        }
    }
}