using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace topdown1;

public class Player
{
    private BoundingBox m_TexBox;

    private float m_XRemainder;
    private float m_YRemainder;

    private Point m_ColliderSize;
    private Point m_ColliderSizeHalf;

    private float m_PlayerSpeed;
    private Texture2D m_Tex;

    public Player(PlayerEntity entity)
    {
        m_TexBox = new BoundingBox((int)entity.Position.X, (int)entity.Position.Y, 16, 16);

        m_ColliderSize = new Point(11, 11);
        m_ColliderSizeHalf = m_ColliderSize.Scale(0.5f);

        m_PlayerSpeed = 150f;
        m_Tex = TextureManager.CharacterTex;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(m_Tex, new Vector2(m_TexBox.Left, m_TexBox.Top), TextureManager.PlayerTexSourceRect, Color.White);

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
                m_TexBox.Left += sign;
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
                m_TexBox.Top += sign;
                pixelAmount -= sign;
            }
        }
    }

    private BoundingBox GetColliderBox()
    {
        // get colliders' top left
        int x = m_TexBox.Left + (int)(m_TexBox.HalfWidth - m_ColliderSizeHalf.X);
        int y = m_TexBox.Top + (int)(m_TexBox.HalfHeight - m_ColliderSizeHalf.Y);

        return new BoundingBox(x, y, m_ColliderSize.X, m_ColliderSize.Y);
    }
}