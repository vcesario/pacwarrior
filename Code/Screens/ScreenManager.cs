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

    private SpriteBatch m_SharedSpriteBatch;
    public static SpriteBatch SharedSpriteBatch => m_Instance.m_SharedSpriteBatch;

    private Texture2D m_BlankTexture;

    private List<AbstractScreen> m_Screens;
    private List<AbstractScreen> m_TempScreensList_Update;
    private List<AbstractScreen> m_TempScreensList_Messages;

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

        m_Screens = new List<AbstractScreen>();
        m_TempScreensList_Update = new List<AbstractScreen>();
        m_TempScreensList_Messages = new List<AbstractScreen>();
    }

    protected override void LoadContent()
    {
        m_SharedSpriteBatch = new SpriteBatch(GraphicsDevice);

        m_BlankTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        m_BlankTexture.SetData(new[] { Color.White });

        // first screen
        // AddScreen(new StartScreen()); // <-- original
        AddScreen(new GameScreen(), new HudScreen()); // <-- debug

        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        // Make a copy of the master screen list, to avoid confusion if
        // the process of updating one screen adds or removes others.
        m_TempScreensList_Update.Clear();

        foreach (AbstractScreen screen in m_Screens)
            m_TempScreensList_Update.Add(screen);

        bool otherScreenHasFocus = !Game.IsActive;
        bool coveredByOtherScreen = false;

        // Loop as long as there are screens waiting to be updated.
        while (m_TempScreensList_Update.Count > 0)
        {
            // Pop the topmost screen off the waiting list.
            AbstractScreen screen = m_TempScreensList_Update[m_TempScreensList_Update.Count - 1];

            m_TempScreensList_Update.RemoveAt(m_TempScreensList_Update.Count - 1);

            // Update the screen.
            screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (screen.ScreenState == ScreenStates.TransitionOn ||
                screen.ScreenState == ScreenStates.Active)
            {
                // If this is the first active screen we came across,
                // give it a chance to handle input.
                if (!otherScreenHasFocus)
                {
                    if (screen.HandleInput(gameTime))
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

    public static AbstractScreen[] GetScreens()
    {
        return m_Instance.m_Screens.ToArray();
    }

    public static bool IsScreenOpen<T>() where T : AbstractScreen
    {
        return m_Instance.m_Screens.Find(_screen => _screen is T) != null;
    }

    public static T GetScreen<T>() where T : AbstractScreen
    {
        T screen = (T)m_Instance.m_Screens.Find(_screen => _screen is T);
        return screen;
    }

    public static void RemoveAllScreens()
    {
        m_Instance.m_Screens.Clear();
    }

    /// <summary>
    /// This method must not be called from another AbstractScreen's <c>Load()</c> method.
    /// </summary>
    public static void AddScreen(params AbstractScreen[] newScreens)
    {
        m_Instance.m_Screens.AddRange(newScreens);
        for (int i = 0; i < newScreens.Length; i++)
            newScreens[i].Load();
    }

    public static void RemoveScreen<T>() where T : AbstractScreen
    {
        bool found = false;
        for (int i = m_Instance.m_Screens.Count - 1; i >= 0 && !found; i--)
        {
            if (m_Instance.m_Screens[i].GetType() == typeof(T))
            {
                m_Instance.m_Screens.RemoveAt(i);
                found = true;
            }
        }
    }

    /// <summary>
    /// Helper draws a translucent black fullscreen sprite, used for fading
    /// screens in and out, and for darkening the background behind popups.
    /// </summary>
    public static void FadeBackBufferToBlack(float alpha)
    {
        SharedSpriteBatch.Draw(m_Instance.m_BlankTexture,
                         new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT),
                         Color.Black * alpha);
    }

    public static void SendMessageToScreens(GameMessages message)
    {
        // Make a copy of the master screen list, to avoid confusion if
        // the process of updating one screen adds or removes others.
        m_Instance.m_TempScreensList_Messages.Clear();

        foreach (AbstractScreen screen in m_Instance.m_Screens)
            m_Instance.m_TempScreensList_Messages.Add(screen);

        // Loop as long as there are screens waiting to be updated.
        while (m_Instance.m_TempScreensList_Messages.Count > 0)
        {
            // Pop the topmost screen off the waiting list.
            AbstractScreen screen = m_Instance.m_TempScreensList_Messages[m_Instance.m_TempScreensList_Messages.Count - 1];

            m_Instance.m_TempScreensList_Messages.RemoveAt(m_Instance.m_TempScreensList_Messages.Count - 1);

            if (screen.ScreenState == ScreenStates.TransitionOn ||
                screen.ScreenState == ScreenStates.Active)
            {
                screen.ReceiveMessage(message);
            }
        }
    }
}