﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Archer
{
    public class ArcherGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ArcherSprite archerSprite;
        private IceGhostSprite [] ghostSprites = new IceGhostSprite[3];
        private bool isGameOver;
        private bool isWin;

        private Texture2D winTexture;
        private Texture2D gameOverTexture;

        public ArcherGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            archerSprite = new ArcherSprite();
            ghostSprites[0] = new IceGhostSprite(new Vector2(400,200));
            ghostSprites[0].setDecisionTimer(0.5);
            ghostSprites[1] = new IceGhostSprite(new Vector2(0, 300));
            ghostSprites[1].setDecisionTimer(0.8);
            ghostSprites[2] = new IceGhostSprite(new Vector2(750, 75));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            archerSprite.LoadContent(Content);
            ghostSprites[0].LoadContent(Content);
            ghostSprites[1].LoadContent(Content);
            ghostSprites[2].LoadContent(Content);
            gameOverTexture = Content.Load<Texture2D>("game-over");
            winTexture = Content.Load<Texture2D>("win");
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
           if(!isGameOver && !isWin)
            {
                archerSprite.Update(gameTime);
                ghostSprites[0].Update(gameTime);
                ghostSprites[1].Update(gameTime);
                ghostSprites[2].Update(gameTime);

                Viewport viewport = GraphicsDevice.Viewport;
                if (archerSprite.Position.X > viewport.Width - 20) archerSprite.Position.X = viewport.Width - 20;
                if (archerSprite.Position.X < 20) archerSprite.Position.X = 20;
                if (archerSprite.Position.Y > viewport.Height - 40) archerSprite.Position.Y = viewport.Height - 40;
                if (archerSprite.Position.Y < 20) archerSprite.Position.Y = 20;

                foreach (IceGhostSprite ghost in ghostSprites)
                {
                    if (ghost.Position.X > viewport.Width - 20) ghost.Position.X = viewport.Width - 20;
                    if (ghost.Position.X < 20) ghost.Position.X = 20;
                    if (ghost.Position.Y > viewport.Height - 20) ghost.Position.Y = viewport.Height - 20;
                    if (ghost.Position.Y < 20) ghost.Position.Y = 20;
                }

                foreach (ArrowSprite arrow in archerSprite.Arrows)
                {
                    foreach (IceGhostSprite ghost in ghostSprites)
                    {
                        if (arrow.Bounds.CollidesWith(ghost.Bounds))
                        {
                            ghost.IsHit = true;
                            archerSprite.didHit = true;
                        }
                    }
                }
                if (archerSprite.didHit && archerSprite.Arrows.Count>0)
                {
                    archerSprite.Arrows.Dequeue();
                }
                foreach(IceGhostSprite ghostSprite in ghostSprites)
                {
                    if (ghostSprite.Fireball != null && ghostSprite.Fireball.Bounds.CollidesWith(archerSprite.Bounds) || (!ghostSprite.IsDead && archerSprite.Bounds.CollidesWith(ghostSprite.Bounds)))
                    {
                        isGameOver = true;
                    }
                }

                isWin = true;
                foreach (IceGhostSprite ghost in ghostSprites)
                {
                    if (!ghost.IsDead)
                    {
                        isWin = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            foreach (IceGhostSprite ghostSprite in ghostSprites) ghostSprite.Draw(gameTime, _spriteBatch, isGameOver);
            archerSprite.Draw(gameTime, _spriteBatch);
            if (isGameOver) _spriteBatch.Draw(gameOverTexture, new Rectangle(300, 150, 200, 100), Color.White);
            else if (isWin) _spriteBatch.Draw(winTexture, new Rectangle(300, 150, 200, 100), Color.ForestGreen);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
