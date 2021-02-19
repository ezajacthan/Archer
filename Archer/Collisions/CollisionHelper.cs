using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Archer.Collisions
{
    public static class CollisionHelper
    {

        /// <summary>
        /// Detects collision between two bounding circles
        /// </summary>
        /// <param name="a">first bounding circle</param>
        /// <param name="b">second bounding circle</param>
        /// <returns>returns true for collision</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects collision between two rectangles
        /// </summary>
        /// <param name="a">the first rectangle</param>
        /// <param name="b">the second rectangle</param>
        /// <returns>true if collision, else false</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
        }

        /// <summary>
        /// Detects collision between rectangle and circle
        /// </summary>
        /// <param name="c">the circle</param>
        /// <param name="r">the rectangle</param>
        /// <returns>true if collision, false if not</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);

            return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearX, 2) + Math.Pow(c.Center.Y - nearY, 2);
        }
    }
}
