using System;
using LDtk;
using Microsoft.Xna.Framework;

namespace topdown1;

public class EnemySpawnerEntity : ILDtkEntity
{
    public string Identifier { get; set; }
    public Guid Iid { get; set; }
    public int Uid { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public Vector2 Pivot { get; set; }
    public Rectangle Tile { get; set; }
    public Color SmartColor { get; set; }
}