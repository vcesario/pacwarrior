using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using topdown1;

public class Player
{
    public PlayerEntity SourceEntity { get; private set; }

    private Point m_Position; // top left pixel

    private int m_ColliderWidth;
    private int m_ColliderHeight;
    private float m_XRemainder;
    private float m_YRemainder;

    private float m_PlayerSpeed = 150f;
    private Texture2D m_Tex;

    public Player(PlayerEntity entity)
    {
        SourceEntity = entity;

        m_Position = entity.Position.ToPoint();

        m_ColliderWidth = 15;
        m_ColliderHeight = 15;

        m_Tex = GameStartup.ContentManager.Load<Texture2D>("textures/tilemap");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(m_Tex, new Vector2(m_Position.X, m_Position.Y), SourceEntity.Tile, Color.White, 0f, SourceEntity.Pivot * SourceEntity.Size, 1f, SpriteEffects.None, 0f);

        if (GameStartup.DebugEnabled)
        {
            Rectangle boundingRect = new Rectangle(m_Position.X, m_Position.Y, m_ColliderWidth, m_ColliderHeight);
            Primitives2D.DrawRectangle(spriteBatch, boundingRect, Color.Red);
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
            BoundingBox playerBox = new BoundingBox(m_Position.X + sign, m_Position.Y, m_ColliderWidth, m_ColliderHeight);

            if (sign > 0) // right side checking
            {
                Point coord1 = MapGrid.ConvertPixelToGrid(playerBox.TopRight);
                Point coord2 = MapGrid.ConvertPixelToGrid(playerBox.BottomRight);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }
            else // left side checking
            {

                Point coord1 = MapGrid.ConvertPixelToGrid(playerBox.TopLeft);
                Point coord2 = MapGrid.ConvertPixelToGrid(playerBox.BottomLeft);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }

            if (!collided)
            {
                m_Position.X += sign;
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
            BoundingBox playerBox = new BoundingBox(m_Position.X, m_Position.Y + sign, m_ColliderWidth, m_ColliderHeight);

            if (sign > 0) // bottom side checking
            {
                Point coord1 = MapGrid.ConvertPixelToGrid(playerBox.BottomLeft);
                Point coord2 = MapGrid.ConvertPixelToGrid(playerBox.BottomRight);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }
            else // top side checking
            {

                Point coord1 = MapGrid.ConvertPixelToGrid(playerBox.TopLeft);
                Point coord2 = MapGrid.ConvertPixelToGrid(playerBox.TopRight);

                if (MapGrid.IsTileAWall(coord1.X, coord1.Y) || MapGrid.IsTileAWall(coord2.X, coord2.Y))
                {
                    collided = true;
                }
            }

            if (!collided)
            {
                m_Position.Y += sign;
                pixelAmount -= sign;
            }
        }
    }
}