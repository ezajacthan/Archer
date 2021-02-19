using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Archer.Collisions
{
    /// <summary>
    /// bounding rectangle for collision detection
    /// </summary>
    public struct BoundingRectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public BoundingRectangle(Vector2 pos, float width, float height)
        {
            X = pos.X;
            Y = pos.Y;
            Width = width;
            Height = height;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingCircle c)
        {
            return CollisionHelper.Collides(c, this);
        }
    }
}
