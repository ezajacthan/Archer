using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Archer.StateManagement;
using Archer.Sprites;


namespace Archer.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager content;

        private ArcherSprite archerSprite;
        private IceGhostSprite[] ghostSprites = new IceGhostSprite[3];
        private GrassTexture grassTexture;

        private Song backgroundSong;

        private bool isGameOver;
        private bool isWin;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            archerSprite = new ArcherSprite();
            ghostSprites[0] = new IceGhostSprite(new Vector2(400, 200));
            ghostSprites[0].setDecisionTimer(0.5);
            ghostSprites[1] = new IceGhostSprite(new Vector2(0, 300));
            ghostSprites[1].setDecisionTimer(0.8);
            ghostSprites[2] = new IceGhostSprite(new Vector2(750, 75));
        }

        //load graphics for game
        public override void Activate()
        {
            if(content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            backgroundSong = content.Load<Song>("forest_sounds");
            MediaPlayer.Play(backgroundSong);
            archerSprite.LoadContent(content);
            foreach (IceGhostSprite ghost in ghostSprites)
            {
                ghost.LoadContent(content);
            }
            grassTexture = new GrassTexture(content.Load<Texture2D>("ground_grass_gen_08"));
        }

        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if(IsActive && !otherScreenHasFocus && !coveredByOtherScreen)
            {
                if (!isGameOver && !isWin)
                {
                    archerSprite.Update(gameTime);
                    foreach (IceGhostSprite ghost in ghostSprites)
                    {
                        if (!ghost.IsDead)
                        {
                            ghost.Update(gameTime);
                        }
                    }

                    if (archerSprite.Position.X > Constants.PLAYER_GAME_WIDTH) archerSprite.Position.X = Constants.PLAYER_GAME_WIDTH;
                    if (archerSprite.Position.X < 0) archerSprite.Position.X = 0;
                    if (archerSprite.Position.Y > Constants.PLAYER_GAME_HEIGHT) archerSprite.Position.Y = Constants.PLAYER_GAME_HEIGHT;
                    if (archerSprite.Position.Y < 0) archerSprite.Position.Y = 0;

                    foreach (IceGhostSprite ghost in ghostSprites)
                    {
                        if (ghost.Position.X > 2000) ghost.Position.X = 2000;
                        if (ghost.Position.X < 20) ghost.Position.X = 20;
                        if (ghost.Position.Y > 2000) ghost.Position.Y = 2000;
                        if (ghost.Position.Y < 20) ghost.Position.Y = 20;
                    }

                    foreach (ArrowSprite arrow in archerSprite.Arrows)
                    {
                        foreach (IceGhostSprite ghost in ghostSprites)
                        {
                            if (arrow.Bounds.CollidesWith(ghost.Bounds))
                            {
                                ghost.IsHit = true;
                                ghost.setHitbox(new Vector2(900, 500), 0, 0);
                                archerSprite.didHit = true;
                            }
                        }
                    }
                    foreach (IceGhostSprite ghostSprite in ghostSprites)
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

                    if(isWin)
                    {
                        MediaPlayer.Stop();
                        ScreenManager.RemoveScreen(this);
                        ScreenManager.AddScreen(new WinScreen(), null);
                    }
                    if(isGameOver)
                    {
                        MediaPlayer.Stop();
                        ScreenManager.RemoveScreen(this);
                        ScreenManager.AddScreen(new DeathScreen(), null);
                    }
                }
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            var _spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.GraphicsDevice.Clear(Color.Blue);

            //calculate offset vector
            Vector2 offset = new Vector2(200, 200);
            offset -= archerSprite.Position;
            offset.X = MathHelper.Clamp(offset.X, -1248, 0);
            offset.Y = MathHelper.Clamp(offset.Y, -1568, 0);

            Matrix transform = Matrix.CreateTranslation(offset.X, offset.Y, 0);

            _spriteBatch.Begin(transformMatrix: transform);
            grassTexture.Draw(_spriteBatch);
            foreach (IceGhostSprite ghost in ghostSprites)
            {
                if (!ghost.IsDead)
                {
                    ghost.Draw(gameTime, _spriteBatch, isGameOver);
                }
            }
            archerSprite.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}
