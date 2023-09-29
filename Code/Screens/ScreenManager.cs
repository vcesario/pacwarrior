using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public enum ScreenStates
{
    Active,
    TransitionOn,
    Hidden,
}

public class ScreenManager : DrawableGameComponent
{
    // ** CONSTANT PROPERTIES

    // -- these values match the LDTk settings, but shouldn't be used in any other part of the code
    // since this is only for window setup and it's just a "coincidence" that they match
    // basically, i want the window to be the size of the map, but I need the values before the map is initialized
    private const int _TileSize = 16;
    private const int _ScreenWidthInTiles = 3 * 16;
    private const int _ScreenHeightInTiles = 3 * 9;
    // --

    public const int SCREEN_WIDTH = _ScreenWidthInTiles * _TileSize;
    public const int SCREEN_HEIGHT = _ScreenHeightInTiles * _TileSize;
    // **

    private InputState m_Input;

    private SpriteBatch m_SharedSpriteBatch;
    public static SpriteBatch SharedSpriteBatch => m_Instance.m_SharedSpriteBatch;

    private Texture2D m_BlankTexture;

    private List<AbstractScreen> m_Screens;
    private List<AbstractScreen> m_TempScreensList;

    private bool m_TraceEnabled;

    public static GraphicsDevice SharedGraphicsDevice => m_Instance.GraphicsDevice;

    private static ScreenManager m_Instance;

    public ScreenManager(GameStartup game) : base(game)
    {
        m_Instance = this;

        // applying screen size
        GameStartup.Graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
        GameStartup.Graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
        GameStartup.Graphics.ApplyChanges();

        game.IsMouseVisible = true;

        m_Input = new InputState();
        m_Screens = new List<AbstractScreen>();
        m_TempScreensList = new List<AbstractScreen>();
    }

    // public override void Initialize()
    // {
    //     base.Initialize();
    // }

    protected override void LoadContent()
    {
        m_SharedSpriteBatch = new SpriteBatch(GraphicsDevice);

        m_BlankTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        m_BlankTexture.SetData(new[] { Color.White });

        // first screen
        AddScreen(new StartScreen()); // <-- original
        // AddScreen(new GameScreen(m_SharedSpriteBatch), new HudScreen()); // <-- debug

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
                    if (screen.HandleInput(gameTime, m_Input))
                        otherScreenHasFocus = true;
                }

                // If this is an active non-popup, inform any subsequent
                // screens that they are covered by it.
                if (!screen.IsPopup)
                    coveredByOtherScreen = true;
            }
        }

        base.Update(gameTime); // @QUESTION: do I need to call this?

        if (m_TraceEnabled) // Print debug trace?
            TraceScreens();
    }


    /// <summary>
    /// Prints a list of all the screens, for debugging.
    /// </summary>
    private void TraceScreens()
    {
        List<string> screenNames = new List<string>();

        foreach (AbstractScreen screen in m_Screens)
            screenNames.Add(screen.GetType().Name);

        Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
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

    // public void RemoveScreenWithTransition(AbstractScreen screen, Action onTransitionEnded = null)
    // {
    //     // screen animation
    //     m_Screens.Remove(screen);
    //     onTransitionEnded?.Invoke();
    // }
    // public void AddScreenWithTransition(AbstractScreen screen, Action onTransitionEnded = null)
    // {
    //     m_Screens.Add(screen);
    //     screen.Load();
    //     // screen animation
    //     onTransitionEnded?.Invoke();
    // }

    public static void RemoveAllScreens()
    {
        m_Instance.m_Screens.Clear();
    }
    public static void AddScreen(params AbstractScreen[] screens)
    {
        m_Instance.m_Screens.AddRange(screens);
        for (int i = 0; i < screens.Length; i++)
            screens[i].Load();
    }

    // public GameScreen InstantiateGameScreen()
    // {
    //     return new GameScreen(this, m_SharedSpriteBatch);
    // }

    // public StartScreen InstantiateStartScreen()
    // {
    //     return new StartScreen(this);
    // }

    // public HudScreen InstantiateHudScreen()
    // {
    //     return new HudScreen(this);
    // }

    // #region Screen Calls
    // public void OpenGameplay()
    // {
    //     AddScreenWithTransition(InstantiateGameScreen());
    // }
    // #endregion

    /// <summary>
    /// Helper draws a translucent black fullscreen sprite, used for fading
    /// screens in and out, and for darkening the background behind popups.
    /// </summary>
    public void FadeBackBufferToBlack(float alpha)
    {
        Viewport viewport = GraphicsDevice.Viewport;

        m_SharedSpriteBatch.Begin();

        m_SharedSpriteBatch.Draw(m_BlankTexture,
                         new Rectangle(0, 0, viewport.Width, viewport.Height),
                         Color.Black * alpha);

        m_SharedSpriteBatch.End();
    }
}