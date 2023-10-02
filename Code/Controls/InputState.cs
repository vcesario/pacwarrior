using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace topdown1;

// @TODO: Assert that every InputCommand is present in GetPressed, GetPressing and GetReleased methods
public static class InputState
{
    private static KeyboardState m_LastKeyboardState, m_CurrentKeyboardState;

    public static void Update(GameTime gameTime)
    {
        m_LastKeyboardState = m_CurrentKeyboardState;
        m_CurrentKeyboardState = Keyboard.GetState();
    }

    public static bool GetPressed(InputCommands command)
    {
        switch (command)
        {
            case InputCommands.UP:
                return (m_CurrentKeyboardState.IsKeyDown(Keys.Up) && m_LastKeyboardState.IsKeyUp(Keys.Up))
                        || (m_CurrentKeyboardState.IsKeyDown(Keys.W) && m_LastKeyboardState.IsKeyUp(Keys.W));
            case InputCommands.LEFT:
                return (m_CurrentKeyboardState.IsKeyDown(Keys.Left) && m_LastKeyboardState.IsKeyUp(Keys.Left))
                        || (m_CurrentKeyboardState.IsKeyDown(Keys.A) && m_LastKeyboardState.IsKeyUp(Keys.A));
            case InputCommands.RIGHT:
                return (m_CurrentKeyboardState.IsKeyDown(Keys.Right) && m_LastKeyboardState.IsKeyUp(Keys.Right))
                        || (m_CurrentKeyboardState.IsKeyDown(Keys.D) && m_LastKeyboardState.IsKeyUp(Keys.D));
            case InputCommands.DOWN:
                return (m_CurrentKeyboardState.IsKeyDown(Keys.Down) && m_LastKeyboardState.IsKeyUp(Keys.Down))
                        || (m_CurrentKeyboardState.IsKeyDown(Keys.S) && m_LastKeyboardState.IsKeyUp(Keys.S));

            case InputCommands.UI_SUBMIT:
                return m_CurrentKeyboardState.IsKeyDown(Keys.Space) && m_LastKeyboardState.IsKeyUp(Keys.Space);
            case InputCommands.UI_EXIT:
                return m_CurrentKeyboardState.IsKeyDown(Keys.Escape) && m_LastKeyboardState.IsKeyUp(Keys.Escape);

            case InputCommands.DEBUG_1:
                return m_CurrentKeyboardState.IsKeyDown(Keys.F1) && m_LastKeyboardState.IsKeyUp(Keys.F1);
            default:
                break;
        }

        return false;
    }
    public static bool GetPressing(InputCommands command)
    {
        switch (command)
        {
            case InputCommands.UP:
                return m_CurrentKeyboardState.IsKeyDown(Keys.Up) || m_CurrentKeyboardState.IsKeyDown(Keys.W);
            case InputCommands.LEFT:
                return m_CurrentKeyboardState.IsKeyDown(Keys.Left) || m_CurrentKeyboardState.IsKeyDown(Keys.A);
            case InputCommands.RIGHT:
                return m_CurrentKeyboardState.IsKeyDown(Keys.Right) || m_CurrentKeyboardState.IsKeyDown(Keys.D);
            case InputCommands.DOWN:
                return m_CurrentKeyboardState.IsKeyDown(Keys.Down) || m_CurrentKeyboardState.IsKeyDown(Keys.S);

            case InputCommands.UI_SUBMIT:
                return m_CurrentKeyboardState.IsKeyDown(Keys.Space);
            case InputCommands.UI_EXIT:
                return m_CurrentKeyboardState.IsKeyDown(Keys.Escape);

            case InputCommands.DEBUG_1:
                return m_CurrentKeyboardState.IsKeyDown(Keys.F1);
            default:
                break;
        }

        return false;
    }
    public static bool GetReleased(InputCommands command)
    {
        switch (command)
        {
            case InputCommands.UP:
                return (m_CurrentKeyboardState.IsKeyUp(Keys.Up) && m_LastKeyboardState.IsKeyDown(Keys.Up))
                        || (m_CurrentKeyboardState.IsKeyUp(Keys.W) && m_LastKeyboardState.IsKeyDown(Keys.W));
            case InputCommands.LEFT:
                return (m_CurrentKeyboardState.IsKeyUp(Keys.Left) && m_LastKeyboardState.IsKeyDown(Keys.Left))
                        || (m_CurrentKeyboardState.IsKeyUp(Keys.A) && m_LastKeyboardState.IsKeyDown(Keys.A));
            case InputCommands.RIGHT:
                return (m_CurrentKeyboardState.IsKeyUp(Keys.Right) && m_LastKeyboardState.IsKeyDown(Keys.Right))
                        || (m_CurrentKeyboardState.IsKeyUp(Keys.D) && m_LastKeyboardState.IsKeyDown(Keys.D));
            case InputCommands.DOWN:
                return (m_CurrentKeyboardState.IsKeyUp(Keys.Down) && m_LastKeyboardState.IsKeyDown(Keys.Down))
                        || (m_CurrentKeyboardState.IsKeyUp(Keys.S) && m_LastKeyboardState.IsKeyDown(Keys.S));

            case InputCommands.UI_SUBMIT:
                return m_CurrentKeyboardState.IsKeyUp(Keys.Space) && m_LastKeyboardState.IsKeyDown(Keys.Space);
            case InputCommands.UI_EXIT:
                return m_CurrentKeyboardState.IsKeyUp(Keys.Escape) && m_LastKeyboardState.IsKeyDown(Keys.Escape);

            case InputCommands.DEBUG_1:
                return m_CurrentKeyboardState.IsKeyUp(Keys.F1) && m_LastKeyboardState.IsKeyDown(Keys.F1);
            default:
                break;
        }

        return false;
    }
}