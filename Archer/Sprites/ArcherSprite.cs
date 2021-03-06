using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Archer.Collisions;

namespace Archer
{
    public enum Direction
    {
        Down = 2,
        Right = 1,
        Up = 4,
        Left = 3,
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
        private KeyboardState priorKeyboardState;

        private Texture2D drawTexture;
        private Rectangle source;
        private BoundingRectangle hitbox = new BoundingRectangle(206,208,15,23);
        public BoundingRectangle Bounds => hitbox;

        private Texture2D idleTextureBack;
        private Texture2D idleTextureFront;
        private Texture2D idleTextureSide;
        private Texture2D walkTextureBack;
        private Texture2D walkTextureFront;
        private Texture2D walkTextureSide;
        private Texture2D shootTextureBack;
        private Texture2D shootTextureFront;
        private Texture2D shootTextureSide;
        private Texture2D arrowTexture;
        private Texture2D debugTexture;

        public Vector2 Position = new Vector2(200, 200);
        private double velocity = 0;
        private double acceleration = 0;

        private Direction direction = Direction.Right;
        private Action currAction = Action.Idle;
        private bool flipped;
        private float scaling = 1.5f;
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
        private bool stopUp = false;
        private bool stopDown = false;
        private bool stopLeft = false;
        private bool stopRight = false;
        private bool didShoot;
        private bool canShoot;
        private short shootAnimFrame;
        private short walkAnimFrame;

        private SoundEffect shootSound;
        public Queue<ArrowSprite> Arrows = new Queue<ArrowSprite>();
        public bool didHit;

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
            shootSound = content.Load<SoundEffect>("archerShoot");
            arrowTexture = content.Load<Texture2D>("arrow");
            debugTexture = content.Load<Texture2D>("debug");

            drawTexture = idleTextureFront;
    }

        /// <summary>
        /// helper function to contain walk animation logic
        /// </summary>
        private void Walk(GameTime gameTime, Direction dir)
        {
            moveFrameRate = 0.15f;
            currAction = Action.Walk; 
            if(walkFirstTime)
            {
                walkTimer = gameTime.TotalGameTime.TotalSeconds;
                walkFirstTime = false;
            }

            switch(dir)
            {
                case Direction.Down:
                    acceleration = 0;
                    velocity = 3f;
                    Position.Y = Position.Y + (float)velocity*(float)gameTime.ElapsedGameTime.TotalSeconds * 10;
                    hitbox.Y = Position.Y + 8;
                    break;
                case Direction.Up:
                    acceleration = 0;
                    velocity = 3f;
                    Position.Y = Position.Y - (float) velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
                    hitbox.Y = Position.Y + 8;
                    break;
                case Direction.Right:
                    acceleration = 0;
                    velocity = 3f;
                    Position.X = Position.X + (float)velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
                    hitbox.X = Position.X + 6;
                    break;
                case Direction.Left:
                    acceleration = 0;
                    velocity = 3f;
                    Position.X = Position.X - (float)velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
                    hitbox.X = Position.X + 6;
                    break;
            }
        }

        /// <summary>
        /// Helper function to contain the run animation logic
        /// </summary>
        /// <param name="gameTime"></param>
        private void Run(GameTime gameTime, Direction dir)
        {
            acceleration = 1.66666f;
            velocity = velocity + acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds*100;
            if (velocity > 40) velocity = 40;

            moveFrameRate -= 0.005f;
            if (moveFrameRate < 0.03) moveFrameRate = 0.03f;
            currAction = Action.Run;
            if (walkFirstTime)
            {
                walkTimer = gameTime.TotalGameTime.TotalSeconds;
                walkFirstTime = false;
            }

            //update position based on velocity
            switch (dir)
            {
                case Direction.Down:
                    Position.Y = Position.Y + (float)velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                    hitbox.Y = Position.Y + 8;
                    break;
                case Direction.Up:
                    Position.Y = Position.Y - (float)velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                    hitbox.Y = Position.Y + 8;
                    break;
                case Direction.Right:
                    Position.X = Position.X + (float)velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                    hitbox.X = Position.X + 6;
                    break;
                case Direction.Left:
                    Position.X = Position.X - (float)velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                    hitbox.X = Position.X + 6;
                    break;                   
            }
        }

        /// <summary>
        /// Update the Archer sprite
        /// </summary>
        /// <param name="gameTime">game time manager to get elapsed time</param>
        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(0);
            priorKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            foreach (ArrowSprite arrow in Arrows)
            {
                arrow.Update(gameTime);
            }

            if ((didShoot) && Arrows.Count>0)
            {
                //remove off-screen arrows
                Vector2 currArrowPos = Arrows.Peek().Position;
                if (currArrowPos.X < 0 || currArrowPos.X > 800
                    || currArrowPos.Y < 0 || currArrowPos.Y > 480 || didHit)
                {
                    Arrows.Dequeue();
                    if (Arrows.Count == 0)
                    {
                        didShoot = false;
                        didHit = false;
                    }
                }
            }

            //get input controls
            // Apply keyboard movement
            if (keyboardState.IsKeyDown(Keys.Up) && !stopUp)
            {
                direction = Direction.Up;
                flipped = false;
                stopDown = true;
                stopLeft = true;
                stopRight = true;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    Run(gameTime, direction);
                }
                else
                {
                    Walk(gameTime, direction);
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Down) && !stopDown)
            {
                direction = Direction.Down;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    Run(gameTime, direction);
                }
                else
                {
                    Walk(gameTime, direction);
                }
                flipped = false;
                stopUp = true;
                stopLeft = true;
                stopRight = true;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && !stopLeft)
            {
                stopDown = true;
                stopUp = true;
                stopRight = true;
                direction = Direction.Left;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    Run(gameTime, direction);
                }
                else
                {
                    Walk(gameTime, direction);
                }
                flipped = true;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && !stopRight)
            {
                stopDown = true;
                stopLeft = true;
                stopUp = true;
                direction = Direction.Right;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    Run(gameTime, direction);
                }
                else
                {
                    Walk(gameTime, direction);
                }
                flipped = false;
            }
            else
            {
                stopDown = false ;
                stopLeft = false;
                stopRight = false;
                stopUp = false;
            }

            if(keyboardState.IsKeyDown(Keys.Space))
            {
                //space shoots the bow

                //Can't move while shooting
                stopDown = true;
                stopLeft = true;
                stopRight = true;
                stopUp = true;
                acceleration = 0;
                velocity = 0;

                currAction = Action.Shoot;
                if (shootFirstTime)
                {
                    didShoot = false;
                    shootTimer = gameTime.TotalGameTime.TotalSeconds;
                    shootFirstTime = false;
                }
            }
            if (keyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyUp(Keys.Up)
                && keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.Right))
            {
                //reset action
                currAction = Action.Idle;
                acceleration = 0;
                velocity = 0;
            }
            if(keyboardState.IsKeyUp(Keys.Space))
            { 
                if(priorKeyboardState.IsKeyDown(Keys.Space) && canShoot)
                {
                    canShoot = false;
                    shootSound.Play();
                    Vector2 arrowPosition = new Vector2(Position.X + 25, Position.Y + 16);
                    ArrowSprite newArrow = new ArrowSprite(direction, flipped, arrowTexture, arrowPosition);
                    Arrows.Enqueue(newArrow);
                    didShoot = true;
                }
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
            if (keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.Down))
            {
                acceleration = 0;
                velocity = 0;
            }
            if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left))
            {
                acceleration= 0;
                velocity= 0;
            }


            //update texture info
            switch (currAction)
            {
                case (Action.Idle):
                    if (direction == Direction.Up)
                    {
                        drawTexture = idleTextureBack;
                        source = idleBackSource;
                        hitbox.Width = idleBackSource.Width;
                        hitbox.Height = idleBackSource.Height;
                    }
                    else if (direction == Direction.Down)
                    {
                        drawTexture = idleTextureFront;
                        source = idleFrontSource;
                        hitbox.Width = idleFrontSource.Width;
                        hitbox.Height = idleFrontSource.Height;
                    }
                    else
                    {
                        drawTexture = idleTextureSide;
                        source = idleSideSource;
                        hitbox.Width = idleSideSource.Width;
                        hitbox.Height = idleSideSource.Height;
                    }
                    break;
                case (Action.Walk):
                    if (direction == Direction.Up)
                    {
                        drawTexture = walkTextureBack;
                        source = walkBackSource;
                        hitbox.Width = walkBackSource.Width;
                        hitbox.Height = walkBackSource.Height;
                    }
                    else if (direction == Direction.Down)
                    {
                        drawTexture = walkTextureFront;
                        source = walkFrontSource;
                        hitbox.Width = walkFrontSource.Width;
                        hitbox.Height = walkFrontSource.Height;
                    }
                    else
                    {
                        drawTexture = walkTextureSide;
                        source = walkSideSource;
                        hitbox.Width = walkSideSource.Width;
                        hitbox.Height = walkSideSource.Height;
                    }
                    break;
                case (Action.Run):
                    if (direction == Direction.Up)
                    {
                        drawTexture = walkTextureBack;
                        source = walkBackSource;
                        hitbox.Width = walkBackSource.Width;
                        hitbox.Height = walkBackSource.Height;
                    }
                    else if (direction == Direction.Down)
                    {
                        drawTexture = walkTextureFront;
                        source = walkFrontSource;
                        hitbox.Width = walkFrontSource.Width;
                        hitbox.Height = walkFrontSource.Height;
                    }
                    else
                    {
                        drawTexture = walkTextureSide;
                        source = walkSideSource;
                        hitbox.Width = walkSideSource.Width;
                        hitbox.Height = walkSideSource.Height;
                    }
                    break;
                case (Action.Shoot):
                    if (direction == Direction.Up)
                    {
                        drawTexture = shootTextureBack;
                        source = shootBackSource;
                        hitbox.Width = shootBackSource.Width;
                        hitbox.Height = shootBackSource.Height;
                    }
                    else if (direction == Direction.Down)
                    {
                        drawTexture = shootTextureFront;
                        source = shootFrontSource;
                        hitbox.Width = shootFrontSource.Width;
                        hitbox.Height = shootFrontSource.Height;
                    }
                    else
                    {
                        drawTexture = shootTextureSide;
                        source = shootSideSource;
                        hitbox.Width = shootSideSource.Width;
                        hitbox.Height = shootSideSource.Height;
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
                    canShoot = true;
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
            foreach (ArrowSprite arrow in Arrows)
            {
                arrow.Draw(spriteBatch);
            }
            spriteBatch.Draw(drawTexture, Position, source, Color.White, 0, new Vector2(0, 0), scaling, flip, 0);
        }
    }

   
}
