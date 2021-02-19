using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Archer.Collisions;

namespace Archer
{
    public class FireballSprite
    {
        private Rectangle sourceRectangle = new Rectangle(21,30,76,48);
        private BoundingCircle hitbox;

        public BoundingCircle Bounds => hitbox;

        /// <summary>
        /// Propoerty to hold the position of the sprite
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// property to hold the texture of the sprite
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// property to hold whether the sprite gets flipped or not
        /// </summary>
        public bool Flipped { get; set; }

        /// <summary>
        /// property to hold which direction the sprite is facing
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// public constructor that sets the direction and whether or not to flip the sprite
        /// </summary>
        /// <param name="dir">direction the sprite should face</param>
        /// <param name="flip">true if the sprite should be flipped, otherwise false</param>
        public FireballSprite(Direction dir, bool flip, Texture2D texture, Vector2 pos)
        {
            Direction = dir;
            Flipped = flip;
            Texture = texture;
            Position = pos;
            hitbox =  new BoundingCircle(new Vector2(pos.X, pos.Y), 22);
        }

        /// <summary>
        /// updates the sprite
        /// </summary>
        /// <param name="gameTime">the game time manager</param>
        public void Update (GameTime gameTime)
        {
            if(Flipped)
            {
                Position += new Vector2(-1, 0) * 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                hitbox.Center += new Vector2(-1, 0) * 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                Position += new Vector2(1, 0) * 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                hitbox.Center += new Vector2(1, 0) * 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = (!Flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color.LightSteelBlue, 0, new Vector2(0, 0),.25f, spriteEffect, 0);
        }
    }
}
