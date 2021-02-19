﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Archer.Collisions;

namespace Archer
{
    public class ArrowSprite
    {
        private BoundingRectangle hitbox;

        /// <summary>
        /// Property to get the hitbox of the sprite
        /// </summary>
        public BoundingRectangle Bounds => hitbox;

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
        public ArrowSprite(Direction dir, bool flip, Texture2D texture, Vector2 pos)
        {
            Direction = dir;
            Flipped = flip;
            Texture = texture;
            Position = pos;
            if (this.Direction == Direction.Down || this.Direction == Direction.Up)
            {
                hitbox = new BoundingRectangle(Position,5,19);
            }
            else
            {
                hitbox = new BoundingRectangle(Position, 19, 5);
            }
        }

        /// <summary>
        /// updates the sprite
        /// </summary>
        /// <param name="gameTime">the game time manager</param>
        public void Update(GameTime gameTime)
        {
            switch (Direction)
            {
                case Direction.Down:
                    Position += new Vector2(0, 1) * 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    hitbox.X = Position.X-7;
                    hitbox.Y = Position.Y-28;
                    break;
                case Direction.Up:
                    Position += new Vector2(-0,-1) * 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    hitbox.X = Position.X;
                    hitbox.Y = Position.Y+7;
                    break;
                case Direction.Right:
                    Position += new Vector2(1, 0) * 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    hitbox.X = Position.X - 28;
                    hitbox.Y = Position.Y;
                    break;
                case Direction.Left:
                    Position += new Vector2(-1, 0) * 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    hitbox.X = Position.X+7;
                    hitbox.Y = Position.Y-7;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = (!Flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(Texture, Position, null, Color.LightSteelBlue, (int)Direction * MathHelper.PiOver2, new Vector2(0, 0), 1.5f, spriteEffect, 0);
        }
    }
}