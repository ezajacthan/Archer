using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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

        private Random random = new Random();

        private Rectangle deathSource = new Rectangle(0,0,32,32);
        private Rectangle attackSource = new Rectangle(0,33,32,32);
        private Rectangle idleSource = new Rectangle(0,65,32,32);

        private GhostAction currAction = GhostAction.Attack;
        private Direction direction = Direction.Right;
        private bool flipped;
        private bool attackReset;
        private bool canWalk;
        private float scaling = 3f;

        private double animTimer;
        private double decisionTimer;
        private short decisionCounter = 0;
        private short shootAnimFrame;
        private short walkAnimFrame;

        /// <summary>
        /// Loads the IceGhost sprite
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            drawTexture = content.Load<Texture2D>("ghostIce_all");
        }

        /// <summary>
        /// Update manager for the ghost sprite
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            decisionTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (attackReset) currAction = GhostAction.Idle;
            if (decisionTimer > 1.0 || attackReset)
            {
                switch (currAction)
                {
                    case GhostAction.Attack:
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
                        if (decisionCounter > 2)
                        {
                            decisionCounter = 0;
                            currAction = GhostAction.Attack;
                        }
                        direction = (Direction)random.Next(0, 4);
                        break;

                    case GhostAction.Death:
                        source = deathSource;
                        canWalk = false;
                        color = Color.White;
                        break;
                }
                decisionTimer -= 1.0;
            }

            if (currAction == GhostAction.Walk && canWalk)
            {
                float scale = 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                switch (direction)
                {
                    case Direction.Up:
                        position += new Vector2(0, -1) * scale;
                        break;
                    case Direction.Down:
                        position += new Vector2(0, 1) * scale;
                        break;
                    case Direction.Left:
                        flipped = true;
                        position += new Vector2(-1, 0) * scale;
                        break;
                    case Direction.Right:
                        flipped = false;
                        position += new Vector2(1, 0) * scale;
                        break;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            /*if (currAction == GhostAction.Attack || currAction == GhostAction.Walk)*/ animTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animTimer > 0.2 && currAction == GhostAction.Attack)
            {
                shootAnimFrame++;
                if (shootAnimFrame > 5)
                {
                    //reset sprite
                    shootAnimFrame = 0;
                    attackReset = true;
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
            /*else animTimer -= gameTime.ElapsedGameTime.TotalSeconds;*/

            SpriteEffects flip = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(drawTexture, position, source, color, 0, new Vector2(0, 0), scaling, flip, 0);
        }
    }
}
