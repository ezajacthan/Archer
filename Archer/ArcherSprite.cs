﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Archer
{
    public enum Direction
    {
        Down,
        Right,
        Up,
        Left,
    }

    public enum Action
    {
        Idle,
        Walk,
        Shoot,
    }

    public class ArcherSprite
    {
        private GamePadState gamePadState;
        private KeyboardState keyboardState;

        private Texture2D drawTexture;
        private Rectangle source;

        private Texture2D idleTextureBack;
        private Texture2D idleTextureFront;
        private Texture2D idleTextureSide;
        private Texture2D walkTextureBack;
        private Texture2D walkTextureFront;
        private Texture2D walkTextureSide;
        private Texture2D shootTextureBack;
        private Texture2D shootTextureFront;
        private Texture2D shootTextureSide;

        private Vector2 position = new Vector2(200, 200);

        private Direction direction = Direction.Right;
        private Action currAction = Action.Idle;
        private bool flipped;
        private float scaling = 2.5f;

        private Rectangle idleSideSource = new Rectangle(8,7,15, 23);
        private Rectangle idleFrontSource = new Rectangle(9,7,13,23);
        private Rectangle idleBackSource = new Rectangle(9,8,13,22);
        private Rectangle walkFrontSource;
        private Rectangle walkBackSource;
        private Rectangle walkSideSource;
        private Rectangle shootFrontSource;
        private Rectangle shootBackSource;
        private Rectangle shootSideSource = new Rectangle(7,6,16,25);

        private double animTimer;
        private short animFrame;

        /// <summary>
        /// Load all of the spritesheets for the different animations
        /// </summary>
        /// <param name="content">The content manager with which to load the sprites</param>
        public void LoadContent(ContentManager content)
        {
            idleTextureFront = content.Load<Texture2D>("hero-idle-front");
            idleTextureBack = content.Load<Texture2D>("hero-idle-back");
            idleTextureSide = content.Load<Texture2D>("hero-idle-side");
            walkTextureFront = content.Load<Texture2D>("hero-walk-front");
            walkTextureBack = content.Load<Texture2D>("hero-back-walk");
            walkTextureSide = content.Load<Texture2D>("hero-walk-side");
            shootTextureFront = content.Load<Texture2D>("hero-attack-front-weapon");
            shootTextureBack = content.Load<Texture2D>("hero-attack-back-weapon");
            shootTextureSide = content.Load<Texture2D>("hero-attack-side-weapon");
        }

        /// <summary>
        /// Update the Archer sprite
        /// </summary>
        /// <param name="gameTime">game time manager to get elapsed time</param>
        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            //get input controls
            // Apply keyboard movement
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                direction = Direction.Up;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                direction = Direction.Down;
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                direction = Direction.Left;
                flipped = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                direction = Direction.Right;
                flipped = false;
            }

            //update texture and position
            switch (currAction)
            {
                case (Action.Idle):
                    if (direction == Direction.Up)
                    {
                        drawTexture = idleTextureBack;
                        source = idleBackSource;
                    }
                    else if (direction == Direction.Down)
                    {
                        drawTexture = idleTextureFront;
                        source = idleFrontSource;
                    }
                    else
                    {
                        drawTexture = idleTextureSide;
                        source = idleSideSource;
                    }
                    break;
                case (Action.Walk):
                    if (direction == Direction.Up)
                    {
                        drawTexture = walkTextureBack;
                        source = walkBackSource;
                    }
                    else if (direction == Direction.Down)
                    {
                        drawTexture = walkTextureFront;
                        source = walkFrontSource;
                    }
                    else
                    {
                        drawTexture = walkTextureSide;
                        source = walkSideSource;
                    }
                    break;
                case (Action.Shoot):
                    if (direction == Direction.Up)
                    {
                        drawTexture = shootTextureBack;
                        source = shootBackSource;
                    }
                    else if (direction == Direction.Down)
                    {
                        drawTexture = shootTextureFront;
                        source = shootFrontSource;
                    }
                    else
                    {
                        drawTexture = shootTextureSide;
                        source = shootSideSource;
                    }
                    break;
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects flip = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(drawTexture, position, source, Color.White, 0, new Vector2(0, 0), scaling, flip, 0);
        }
    }

   
}