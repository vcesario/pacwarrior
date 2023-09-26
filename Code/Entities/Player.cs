using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using topdown1;

public class Player
{
    public PlayerEntity SourceEntity { get; private set; }

    private BoundingBox m_PlayerBox;

    private float m_XRemainder;
    private float m_YRemainder;

    private Point m_ColliderSize;
    private Point m_ColliderSizeHalf;

    private float m_PlayerSpeed = 150f;
    private Texture2D m_Tex;

    public Player(PlayerEntity entity)
    {
        SourceEntity = entity;

        m_PlayerBox = new BoundingBox((int)entity.Position.X, (int)entity.Position.Y, 16, 16);

        m_ColliderSize = new Point(11, 11);
        m_ColliderSizeHalf = m_ColliderSize.Scale(0.5f);

        m_Tex = GameStartup.ContentManager.Load<Texture2D>("textures/tilemap");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(m_Tex, new Vector2(m_PlayerBox.Left, m_PlayerBox.Top), SourceEntity.Tile, Color.White, 0f, SourceEntity.Pivot * SourceEntity.Size, 1f, SpriteEffects.None, 0f);

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
                Point coord1 = MapGrid.ConvertPixelToGrid(offsettedBox.TopRight);
                Point coord2 = MapGrid.ConvertPixelToGrid(offsettedBox.BottomRight);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }
            else // left side checking
            {
                Point coord1 = MapGrid.ConvertPixelToGrid(offsettedBox.TopLeft);
                Point coord2 = MapGrid.ConvertPixelToGrid(offsettedBox.BottomLeft);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }

            if (!collided)
            {
                m_PlayerBox.Left += sign;
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
                Point coord1 = MapGrid.ConvertPixelToGrid(offsettedBox.BottomLeft);
                Point coord2 = MapGrid.ConvertPixelToGrid(offsettedBox.BottomRight);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }
            else // top side checking
            {
                Point coord1 = MapGrid.ConvertPixelToGrid(offsettedBox.TopLeft);
                Point coord2 = MapGrid.ConvertPixelToGrid(offsettedBox.TopRight);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }

            if (!collided)
            {
                m_PlayerBox.Top += sign;
                pixelAmount -= sign;
            }
        }
    }

    private BoundingBox GetColliderBox()
    {
        // get colliders' top left
        int x = m_PlayerBox.Left + (int)(m_PlayerBox.HalfWidth - m_ColliderSizeHalf.X);
        int y = m_PlayerBox.Top + (int)(m_PlayerBox.HalfHeight - m_ColliderSizeHalf.Y);

        return new BoundingBox(x, y, m_ColliderSize.X, m_ColliderSize.Y);
    }
}