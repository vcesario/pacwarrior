using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using topdown1;

public enum ScreenStates
{
    Active,
    TransitionOn,
    Hidden,
}

public class ScreenManager : DrawableGameComponent
{
    // CONSTANT PROPERTIES
    private const int TILE_SIZE = 16;
    private const int GAME_WIDTH = 3 * 16 * TILE_SIZE;
    private const int GAME_HEIGHT = 3 * 9 * TILE_SIZE;
    // **

    private InputState input = new InputState();

    private SpriteBatch m_SharedSpriteBatch;

    private List<AbstractScreen> m_Screens = new List<AbstractScreen>();
    private List<AbstractScreen> m_TempScreensList = new List<AbstractScreen>();

    public ScreenManager(GameStartup game) : base(game)
    {
        // applying screen size
        game.Graphics.PreferredBackBufferWidth = GAME_WIDTH;
        game.Graphics.PreferredBackBufferHeight = GAME_HEIGHT;
        game.Graphics.ApplyChanges();

        game.IsMouseVisible = true;
    }

    public override void Initialize()
    {
        // first screen
        m_Screens.Add(InstantiateStartScreen());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        m_SharedSpriteBatch = new SpriteBatch(GraphicsDevice);
        m_Screens[0].Load();

        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        // Make a copy of the master screen list, to avoid confusion if
        // the process of updating one screen adds or removes others.
        m_TempScreensList.Clear();

        foreach (AbstractScreen screen in m_Screens)
            m_TempScreensList.Add(screen);

        bool otherScreenHasFocus = !Game.IsActive;
        bool coveredByOtherScreen = false;

        // Loop as long as there are screens waiting to be updated.
        while (m_TempScreensList.Count > 0)
        {
            // Pop the topmost screen off the waiting list.
            AbstractScreen screen = m_TempScreensList[m_TempScreensList.Count - 1];

            m_TempScreensList.RemoveAt(m_TempScreensList.Count - 1);

            // Update the screen.
            screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (screen.ScreenState == ScreenStates.TransitionOn ||
                screen.ScreenState == ScreenStates.Active)
            {
                // If this is the first active screen we came across,
                // give it a chance to handle input.
                if (!otherScreenHasFocus)
                {
                    screen.HandleInput(gameTime, input);

                    otherScreenHasFocus = true;
                }

                // If this is an active non-popup, inform any subsequent
                // screens that they are covered by it.
                if (!screen.IsPopup)
                    coveredByOtherScreen = true;
            }
        }

        base.Update(gameTime); // @QUESTION: do I need to call this?
    }

    public override void Draw(GameTime gameTime)
    {
        m_SharedSpriteBatch.Begin();
        foreach (AbstractScreen screen in m_Screens)
        {
            if (screen.ScreenState == ScreenStates.Hidden)
                continue;

            screen.Draw(gameTime, m_SharedSpriteBatch);
        }
        m_SharedSpriteBatch.End();

        base.Draw(gameTime); // @QUESTION: do I need to call this?
    }

    public AbstractScreen[] GetScreens()
    {
        return m_Screens.ToArray();
    }

    public void RemoveScreenWithTransition(AbstractScreen screen, Action onTransitionEnded)
    {
        // animation
        m_Screens.Remove(screen);
        onTransitionEnded?.Invoke();
    }

    public void AddScreenWithTransition(AbstractScreen screen)
    {
        m_Screens.Add(screen);
        screen.Load();
        // animation
    }

    public GameScreen InstantiateGameScreen()
    {
        return new GameScreen(this, m_SharedSpriteBatch);
    }

    public StartScreen InstantiateStartScreen()
    {
        return new StartScreen(this);
    }
}