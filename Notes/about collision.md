onde deixar a colisao



BoundingBox (struct)
    - X
    - Y
    - Width
    - Height
    - TopLeft => (X, Y)
    - TopRight => (X + Width, Y)
    - BottomLeft => (X, Y - Height)
    - BottomRight => (X + Width, Y - Height)

ConvertPixelToGrid(position) => Vector2Extensions

bool[] WallGrid => entity level ou world... ou collisionmanager


draw bounding box

CollisionManager.IsOverlapping(aabb1, aabb2)

o move tem que ser pixel a pixel. separado por eixo