using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Archer.StateManagement;


namespace Archer.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager content;

        private ArcherSprite archerSprite;
        private IceGhostSprite[] ghostSprites = new IceGhostSprite[3];

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

                archerSprite.LoadContent(content);
                foreach (IceGhostSprite ghost in ghostSprites)
                {
                    ghost.LoadContent(content);
                }
                //ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if(IsActive)
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

                    if (archerSprite.Position.X > Constants.GAME_WIDTH - 20) archerSprite.Position.X = Constants.GAME_WIDTH - 20;
                    if (archerSprite.Position.X < 20) archerSprite.Position.X = 20;
                    if (archerSprite.Position.Y > Constants.GAME_HEIGHT - 40) archerSprite.Position.Y = Constants.GAME_HEIGHT - 40;
                    if (archerSprite.Position.Y < 20) archerSprite.Position.Y = 20;

                    foreach (IceGhostSprite ghost in ghostSprites)
                    {
                        if (ghost.Position.X > Constants.GAME_WIDTH - 20) ghost.Position.X = Constants.GAME_WIDTH - 20;
                        if (ghost.Position.X < 20) ghost.Position.X = 20;
                        if (ghost.Position.Y > Constants.GAME_HEIGHT - 60) ghost.Position.Y = Constants.GAME_HEIGHT - 60;
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
            ScreenManager.GraphicsDevice.Clear(Color.ForestGreen);

            _spriteBatch.Begin();
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
