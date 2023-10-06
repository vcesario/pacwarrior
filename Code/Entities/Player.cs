using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class Player
{
    private float m_XRemainder;
    private float m_YRemainder;

    private Point m_ColliderSize;
    private Point m_ColliderSizeHalf;

    private float m_PlayerSpeed;

    private Point m_StartingPosition;

    public PlayerState State { get; private set; }

    private bool m_IsInvisible;

    public int LivesRemaining { get; private set; }

    public TexRenderer Renderer { get; private set; }
    public Point Position => Renderer.Position;

    public Player(PlayerEntity entity)
    {
        Renderer = new TexRenderer(TextureManager.MainTex, TextureManager.PlayerTexSourceRect, entity.Position.ToPoint());
        m_StartingPosition = Renderer.Position;

        m_ColliderSize = new Point(11, 11);
        m_ColliderSizeHalf = m_ColliderSize.Scale(0.5f);

        m_PlayerSpeed = 150f;

        LivesRemaining = 3;

        State = new PlayerState_Default(this);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!m_IsInvisible)
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