using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using topdown1;

public class Player
{
    public PlayerEntity SourceEntity { get; private set; }
    private Vector2 m_Position;
    private Texture2D m_Tex;
    private float m_PlayerSpeed = 150;

    public Player(PlayerEntity entity)
    {
        SourceEntity = entity;
        m_Position = entity.Position;

        m_Tex = GameStartup.ContentManager.Load<Texture2D>("textures/tilemap");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(m_Tex, m_Position, SourceEntity.Tile, Color.White, 0f, SourceEntity.Pivot * SourceEntity.Size, 1f, SpriteEffects.None, 0f);
    }

    public void Move(Vector2 direction, double frameDuration)
    {
        m_Position += direction * Convert.ToInt32(m_PlayerSpeed * (float)frameDuration);
    }
}