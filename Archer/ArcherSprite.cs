using System;
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
        Run,
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
        private float moveFrameRate = 0.15f;

        private Rectangle idleSideSource = new Rectangle(8,7,15, 23);
        private Rectangle idleFrontSource = new Rectangle(9,7,13,23);
        private Rectangle idleBackSource = new Rectangle(9,8,13,22);
        private Rectangle walkFrontSource = new Rectangle(8,5,14,25);
        private Rectangle walkBackSource = new Rectangle(6,7,17,24);
        private Rectangle walkSideSource = new Rectangle(8,6,15,23);
        private Rectangle shootFrontSource = new Rectangle(41,4,16,26);
        private Rectangle shootBackSource = new Rectangle(37,7,22,24);
        private Rectangle shootSideSource = new Rectangle(37,6,21,25);

        private double walkTimer;
        private double shootTimer;
        private bool shootFirstTime = true;
        private bool walkFirstTime = true;
        private short shootAnimFrame;
        private short walkAnimFrame;

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
        /// helper function to contain walk animation logic
        /// </summary>
        private void Walk(GameTime gameTime)
        {
            moveFrameRate = 0.15f;
            currAction = Action.Walk;
            if(walkFirstTime)
            {
                walkTimer = gameTime.TotalGameTime.TotalSeconds;
                walkFirstTime = false;
            }
        }

        /// <summary>
        /// Helper function to contain the run animation logic
        /// </summary>
        /// <param name="gameTime"></param>
        private void Run(GameTime gameTime)
        {
            moveFrameRate = .03f;
            currAction = Action.Run;
            if (walkFirstTime)
            {
                walkTimer = gameTime.TotalGameTime.TotalSeconds;
                walkFirstTime = false;
            }
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
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                direction = Direction.Up;
                flipped = false;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    Run(gameTime);
                }
                else
                {
                    Walk(gameTime);
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                direction = Direction.Down;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    Run(gameTime);
                }
                else
                {
                    Walk(gameTime);
                }
                flipped = false;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                direction = Direction.Left;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    Run(gameTime);
                }
                else
                {
                    Walk(gameTime);
                }
                flipped = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                direction = Direction.Right;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    Run(gameTime);
                }
                else
                {
                    Walk(gameTime);
                }
                flipped = false;
            }
            if(keyboardState.IsKeyDown(Keys.Space))
            {
                //space shoots the bow
                currAction = Action.Shoot;
                if (shootFirstTime)
                {
                    shootTimer = gameTime.TotalGameTime.TotalSeconds;
                    shootFirstTime = false;
                }
            }
            if (keyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyUp(Keys.Up)
                && keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.Right))
            {
                //reset action
                currAction = Action.Idle;
            }
            if(keyboardState.IsKeyUp(Keys.Space))
            { 
                shootSideSource.X = 37;
                shootFrontSource.X = 41;
                shootBackSource.X = 37;
                shootFirstTime = true;
            }
            if (keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.Down) 
                && keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.Right))
            {
                walkFirstTime = true;
                walkSideSource.X = 8;
                walkFrontSource.X = 8;
                walkBackSource.X = 6;
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
                case (Action.Run):
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
            double shootTimerAfter = gameTime.TotalGameTime.TotalSeconds;
            double walkAnimTimer = gameTime.TotalGameTime.TotalSeconds;
            if(shootTimerAfter-shootTimer >= 0.7 && currAction == Action.Shoot)
            {
                shootAnimFrame++;
                if (shootAnimFrame > 1)
                {
                    shootSideSource.X = 67;
                    shootFrontSource.X = 72;
                    shootBackSource.X = 69;
                }
            }
            else if (walkAnimTimer-walkTimer >= moveFrameRate && (currAction == Action.Walk || currAction == Action.Run))
            {
                walkAnimFrame++;
                if (walkAnimFrame > 5) walkAnimFrame = 0;
                walkSideSource.X = 32 * walkAnimFrame + 8;
                walkFrontSource.X = 32 * walkAnimFrame + 8;
                walkBackSource.X = 32* walkAnimFrame +8;
                walkTimer += moveFrameRate;
            }

            SpriteEffects flip = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(drawTexture, position, source, Color.White, 0, new Vector2(0, 0), scaling, flip, 0);
        }
    }

   
}
