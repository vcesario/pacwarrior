using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class Player
{
    // private BoundingBox m_TexBox;

    private float m_XRemainder;
    private float m_YRemainder;

    private Point m_ColliderSize;
    private Point m_ColliderSizeHalf;

    private float m_PlayerSpeed;
    // private Texture2D m_Tex;

    private Point m_StartingPosition;

    public PlayerState State { get; private set; }

    private bool m_IsInvisible;

    public int LivesRemaining { get; private set; }

    // public Color SpriteColor { get; private set; }
    public TexRenderer Renderer { get; private set; }

    public Player(PlayerEntity entity)
    {
        Renderer = new TexRenderer(TextureManager.CharacterTex, TextureManager.PlayerTexSourceRect, entity.Position.ToPoint());
        m_StartingPosition = Renderer.Position;

        // m_TexBox = new BoundingBox((int)entity.Position.X, (int)entity.Position.Y, 16, 16);

        m_ColliderSize = new Point(11, 11);
        m_ColliderSizeHalf = m_ColliderSize.Scale(0.5f);

        m_PlayerSpeed = 150f;
        // m_Tex = TextureManager.CharacterTex;

        LivesRemaining = 3;
        // ScreenManager.SendMessageToScreens(GameMessages.PlayerLivesChanged); // this line is not needed I think

        State = new PlayerState_Default(this);

        // SpriteColor = Color.White;
        // Renderer.SetPosition(m_StartingPosition);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!m_IsInvisible)
            // spriteBatch.Draw(m_Tex, new Vector2(m_TexBox.Left, m_TexBox.Top), TextureManager.PlayerTexSourceRect, SpriteColor);
            Renderer.Draw(spriteBatch);

        if (GameStartup.DebugEnabled)
        {
            Primitives2D.DrawRectangle(spriteBatch, GetColliderBox().Rect, Color.Red);
        }
    }

    public void Move(Vector2 direction, double frameDuration)
    {
        if (direction.LengthSquared() == 0)
            return;
        direction.Normalize();

        Vector2 displacement = direction * m_PlayerSpeed * (float)frameDuration;
        MoveX(displacement.X);
        MoveY(displacement.Y);
    }

    private void MoveX(float amount)
    {
        m_XRemainder += amount;

        int pixelAmount = (int)m_XRemainder;
        if (pixelAmount == 0)
            return;

        m_XRemainder -= pixelAmount;
        int sign = Math.Sign(pixelAmount);

        bool collided = false;
        while (collided == false && pixelAmount != 0)
        {
            BoundingBox offsettedBox = GetColliderBox();
            offsettedBox.Left += sign;

            if (sign > 0) // right side checking
            {
                Point coord1 = MapGrid.PositionToGridCoordinate(offsettedBox.TopRight);
                Point coord2 = MapGrid.PositionToGridCoordinate(offsettedBox.BottomRight);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }
            else // left side checking
            {
                Point coord1 = MapGrid.PositionToGridCoordinate(offsettedBox.TopLeft);
                Point coord2 = MapGrid.PositionToGridCoordinate(offsettedBox.BottomLeft);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }

            if (!collided)
            {
                Point currentPosition = Renderer.Position;
                Renderer.SetPosition(new Point(currentPosition.X + sign, currentPosition.Y));
                // m_TexBox.Left += sign;
                pixelAmount -= sign;
            }
        }
    }
    private void MoveY(float amount)
    {
        m_YRemainder += amount;

        int pixelAmount = (int)m_YRemainder;
        if (pixelAmount == 0)
            return;

        m_YRemainder -= pixelAmount;
        int sign = Math.Sign(pixelAmount);

        bool collided = false;
        while (collided == false && pixelAmount != 0)
        {
            BoundingBox offsettedBox = GetColliderBox();
            offsettedBox.Top += sign;

            if (sign > 0) // bottom side checking
            {
                Point coord1 = MapGrid.PositionToGridCoordinate(offsettedBox.BottomLeft);
                Point coord2 = MapGrid.PositionToGridCoordinate(offsettedBox.BottomRight);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }
            else // top side checking
            {
                Point coord1 = MapGrid.PositionToGridCoordinate(offsettedBox.TopLeft);
                Point coord2 = MapGrid.PositionToGridCoordinate(offsettedBox.TopRight);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }

            if (!collided)
            {
                Point currentPosition = Renderer.Position;
                Renderer.SetPosition(new Point(currentPosition.X, currentPosition.Y + sign));
                // m_TexBox.Top += sign;
                pixelAmount -= sign;
            }
        }
    }

    public BoundingBox GetColliderBox()
    {
        // get colliders' top left
        int x = Renderer.Box.Left + (int)(Renderer.Box.HalfWidth - m_ColliderSizeHalf.X);
        int y = Renderer.Box.Top + (int)(Renderer.Box.HalfHeight - m_ColliderSizeHalf.Y);

        return new BoundingBox(x, y, m_ColliderSize.X, m_ColliderSize.Y);
    }

    public void SetState(PlayerState newState)
    {
        State.Exit();
        State = newState;
        State.Enter();
    }

    public void ReturnToStartPosition()
    {
        Renderer.SetPosition(m_StartingPosition);
        // m_TexBox.Top = m_StartingPosition.Y;
        // m_TexBox.Left = m_StartingPosition.X;
    }

    public void SetInvisible(bool value)
    {
        m_IsInvisible = value;
    }

    public void LoseLife()
    {
        if (LivesRemaining > 0)
        {
            LivesRemaining--;
            ScreenManager.SendMessageToScreens(GameMessages.PlayerLivesChanged);
        }
    }
}