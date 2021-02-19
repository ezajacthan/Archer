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
        private Vector2 position = new Vector2(400, 200);
        private Texture2D drawTexture;
        private Rectangle source;
        private Color color = Color.White;
        private Texture2D debugTexture;

        private Random random = new Random();
        private BoundingRectangle hitbox = new BoundingRectangle(new Vector2(416, 208),16,32);
        public BoundingRectangle Bounds => hitbox;

        private Rectangle deathSource = new Rectangle(0,0,32,32);
        private Rectangle attackSource = new Rectangle(0,33,32,32);
        private Rectangle idleSource = new Rectangle(0,65,32,32);

        private GhostAction currAction = GhostAction.Attack;
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
        private FireballSprite fireball;
        private Texture2D fireballTexture;
        private bool didShoot;
        private short shootCounter = 2;

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
            if(didShoot) fireball.Update(gameTime);
            decisionTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (attackReset) currAction = GhostAction.Idle;
            if (decisionTimer > 0.5)
            {
                switch (currAction)
                {
                    case GhostAction.Attack:
                        attackReset = false;
                        source = attackSource;
                        canWalk = false;
                        color = Color.White;
                        if (shootCounter == 0)
                        {
                            currAction = GhostAction.Death;
                            source = deathSource;
                        }
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
                        position += new Vector2(0, -1) * scale;
                        hitbox.Y -= scale;
                        break;
                    case Direction.Down:
                        position += new Vector2(0, 1) * scale;
                        hitbox.Y += scale;
                        break;
                    case Direction.Left:
                        flipped = true;
                        position += new Vector2(-1, 0) * scale;
                        hitbox.X -= scale;
                        break;
                    case Direction.Right:
                        flipped = false;
                        position += new Vector2(1, 0) * scale;
                        hitbox.X += scale;
                        break;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animTimer > 0.1666666 && currAction == GhostAction.Attack)
            {
                shootAnimFrame++;
                if (shootAnimFrame == 4)
                {
                    didShoot = true;
                    fireball = new FireballSprite(direction, flipped, fireballTexture, position);
                    fireballSound.Play();
                }
                if (shootAnimFrame > 5)
                {
                    //reset sprite
                    shootCounter--;
                    shootAnimFrame = 0;
                    attackReset = true;
                    color = Color.Red;
                }
                source.X = 31 * shootAnimFrame + shootAnimFrame;
                //adjust sprite for visual cleannness
                if (shootAnimFrame == 4) source.X++;
                animTimer -= 0.2;
            }
            else if (animTimer >= 0.1 && (currAction == GhostAction.Walk))
            {
                walkAnimFrame++;
                if (walkAnimFrame > 3) walkAnimFrame = 0;
                //update walk animation
                source.X = 32 * walkAnimFrame;
                animTimer -= 0.1;
            }
            else if(animTimer >= 0.07 && currAction == GhostAction.Death)
            {
                deathAnimFrame++;
                if(deathAnimFrame>8)
                {
                    source = new Rectangle(232, 42, 32, 32);
                }
                source.X = 32 * deathAnimFrame;
                animTimer -= 0.07;
            }

            SpriteEffects flip = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(drawTexture, position, source, color, 0, new Vector2(0, 0), scaling, flip, 0);
            Rectangle debug = new Rectangle((int)this.hitbox.X, (int)this.hitbox.Y, (int)this.Bounds.Width, (int)this.Bounds.Height);
            spriteBatch.Draw(debugTexture, debug, Color.White);
            if (didShoot)
            {
                fireball.Draw(spriteBatch);
            }
        }
    }
}
