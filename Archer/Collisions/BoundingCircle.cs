using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Archer.Collisions
{
    /// <summary>
    /// Struct representing circular bounds
    /// </summary>

    public struct BoundingCircle
    {
        /// <summary>
        /// Center of bounding circle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// Radius of bounding circle
        /// </summary>
        public float Radius;

        /// <summary>
        /// Constructs a new Bounding Circle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="rad"></param>
        public BoundingCircle(Vector2 center, float rad)
        {
            Center = center;
            Radius = rad;
        }

        /// <summary>
        /// Tests for collision of this circle with another
        /// </summary>
        /// <param name="other">the other bounding circle</param>
        /// <returns>true if collision, else false</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingRectangle r)
        {
            return CollisionHelper.Collides(this, r);
        }
    }
}
