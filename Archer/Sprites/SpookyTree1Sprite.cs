using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Archer.Collisions;

namespace Archer.Sprites
{
    public class SpookyTree1Sprite
    {
        private Rectangle sourceRect = new Rectangle(4, 39, 68, 107);

        /// <summary>
        /// Propoerty to hold the position of the sprite
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// property to hold the texture of the sprite
        /// </summary>
        public Texture2D Texture { get; set; }

        public SpookyTree1Sprite(Vector2 pos, Texture2D texture)
        {
            Position = pos;
            Texture = texture;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, Position, sourceRect, Color.White);
        }
    }
}
