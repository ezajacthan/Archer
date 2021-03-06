using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Archer.Collisions;

namespace Archer
{
    public enum GhostAction
    {
        Idle,
        Walk,
        Attack,
        Death,
    }

    public class IceGhostSprite
    {
        public Vector2 Position;
        private Texture2D drawTexture;
        private Rectangle source;
        private Color color = Color.White;
        private Texture2D debugTexture;

        private Random random = new Random();
        private BoundingRectangle hitbox;
        public BoundingRectangle Bounds => hitbox;
        public bool IsHit;
        public bool IsDead;

        private Rectangle deathSource = new Rectangle(0, 0, 32, 32);
        private Rectangle attackSource = new Rectangle(0, 33, 32, 32);
        private Rectangle idleSource = new Rectangle(0, 65, 32, 32);

        private GhostAction currAction = GhostAction.Walk;
        private Direction direction = Direction.Right;
        private bool flipped;
        private bool attackReset;
        private bool canWalk;
        private float scaling = 1.5f;

        private double animTimer;
        private double decisionTimer;
        private short decisionCounter = 0;
        private short shootAnimFrame;
        private short walkAnimFrame;
        private short deathAnimFrame;

        private SoundEffect fireballSound;
        public FireballSprite Fireball;
        private Texture2D fireballTexture;
        private bool didShoot;

        public IceGhostSprite(Vector2 pos)
        {
            Position = pos;
            hitbox = new BoundingRectangle(new Vector2(pos.X + 16, pos.Y + 8), 16, 32);
        }

        public void setHitbox(Vector2 pos, int width, int height)
        {
            hitbox.X = pos.X;
            hitbox.Y = pos.Y;
            hitbox.Width = width;
            hitbox.Height = height;
        }

        /// <summary>
        /// setter for the decision timer
        /// </summary>
        /// <param name="time">time to put in the timer</param>
        public void setDecisionTimer(double time)
        {
            decisionTimer = time;
        }



        /// <summary>
        /// Loads the IceGhost sprite
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            fireballTexture = content.Load<Texture2D>("fireball");
            drawTexture = content.Load<Texture2D>("ghostIce_all");
            fireballSound = content.Load<SoundEffect>("ghostFireball");
            debugTexture = content.Load<Texture2D>("debug");
        }

        /// <summary>
        /// Update manager for the ghost sprite
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if(didShoot) Fireball.Update(gameTime);
            if(IsHit)
            {
                currAction = GhostAction.Death;
                source = deathSource;
                IsHit = false;
            }
            decisionTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (attackReset) currAction = GhostAction.Idle;
            if (decisionTimer > 0.5 || IsHit)
            {
                switch (currAction)
                {
                    case GhostAction.Attack:
                        attackReset = false;
                        source = attackSource;
                        canWalk = false;
                        color = Color.White;
                        break;

                    case GhostAction.Idle:
                        source = idleSource;
                        canWalk = false;
                        attackReset = false;
                        currAction = GhostAction.Walk;
                        color = Color.Red;
                        break;

                    case GhostAction.Walk:
                        source = idleSource;
                        canWalk = true;
                        color = Color.White;
                        decisionCounter++;
                        if (decisionCounter > 3)
                        {
                            decisionCounter = 0;
                            currAction = GhostAction.Attack;
                            source = attackSource;
                        }
                        direction = (Direction)random.Next(0, 4);
                        break;

                    case GhostAction.Death:
                        canWalk = false;
                        color = Color.White;
                        hitbox.Width = 0;
                        hitbox.Height = 0;
                        break;
                }
                decisionTimer -= 0.5;
            }

            if (currAction == GhostAction.Walk && canWalk)
            {
                float scale = 75 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                switch (direction)
                {
                    case Direction.Up:
                        Position += new Vector2(0, -1) * scale;
                        break;
                    case Direction.Down:
                        Position += new Vector2(0, 1) * scale;
                        break;
                    case Direction.Left:
                        flipped = true;
                        Position += new Vector2(-1, 0) * scale;
                        break;
                    case Direction.Right:
                        flipped = false;
                        Position += new Vector2(1, 0) * scale;
                        break;
                }
            }
            hitbox.X = Position.X + 16;
            hitbox.Y = Position.Y + 8;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool isGameOver)
        {
            animTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animTimer > 0.1666666 && currAction == GhostAction.Attack && !isGameOver)
            {
                shootAnimFrame++;
                if (shootAnimFrame == 4)
                {
                    didShoot = true;
                    Fireball = new FireballSprite(direction, flipped, fireballTexture, Position);
                    fireballSound.Play();
                }
                if (shootAnimFrame > 5)
                {
                    //reset sprite
                    shootAnimFrame = 0;
                    attackReset = true;
                    color = Color.Red;
                }
                source.X = 31 * shootAnimFrame + shootAnimFrame;
                //adjust sprite for visual cleannness
                if (shootAnimFrame == 4) source.X++;
                animTimer -= 0.2;
            }
            else if (animTimer >= 0.1 && (currAction == GhostAction.Walk) && !isGameOver)
            {
                walkAnimFrame++;
                if (walkAnimFrame > 3) walkAnimFrame = 0;
                //update walk animation
                source.X = 32 * walkAnimFrame;
                animTimer -= 0.1;
            }
            else if(animTimer >= 0.07 && currAction == GhostAction.Death && !isGameOver)
            {
                deathAnimFrame++;
                if(deathAnimFrame>8)
                {
                    source = new Rectangle(232, 42, 32, 32);
                    IsDead = true;
                }
                source.X = 32 * deathAnimFrame;
                animTimer -= 0.07;
            }

            SpriteEffects flip = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(drawTexture, Position, source, color, 0, new Vector2(0, 0), scaling, flip, 0);
            if (didShoot)
            {
                Fireball.Draw(spriteBatch);
            }
        }
    }
}
