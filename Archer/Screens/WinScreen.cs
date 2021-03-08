﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Archer.StateManagement;

namespace Archer.Screens
{
    public class WinScreen : GameScreen
    {
        private ContentManager contentManager;
        private Texture2D texture;
        private InputAction _exit;
        private InputAction _replay;


        public WinScreen()
        {
            _exit = new InputAction(new Buttons[] { Buttons.B}, new Keys[] { Keys.Escape}, true);
            _replay = new InputAction(new Buttons[] { Buttons.Start }, new Keys[] { Keys.Enter }, true);
        }

        public override void Activate()
        {
            if(contentManager == null)
            {
                contentManager = new ContentManager(ScreenManager.Game.Services, "Content");
                texture = contentManager.Load<Texture2D>("win");
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex player;
            if(_exit.Occurred(input, null, out player))
            {
                ScreenManager.Game.Exit();
            }
            if(_replay.Occurred(input, null, out player))
            {
                ScreenManager.AddScreen(new GameplayScreen(), null);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var font = ScreenManager.Font;
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(font, "You Win!", new Vector2(310, 175), Color.Gold);
            ScreenManager.SpriteBatch.DrawString(font, "Press Enter to play again", new Vector2(150, 270), Color.DarkGray);
            ScreenManager.SpriteBatch.DrawString(font, "Press ESC to exit", new Vector2(210, 320), Color.DarkGray);
            ScreenManager.SpriteBatch.End();
        }
    }
}
