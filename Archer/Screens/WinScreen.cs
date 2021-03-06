﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Archer.StateManagement;

namespace Archer.Screens
{
    public class WinScreen : GameScreen
    {
        private ContentManager contentManager;
        private Texture2D texture;
        private InputAction _exit;
        private InputAction _replay;
        private SoundEffect winScreenSound;


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
                winScreenSound = contentManager.Load<SoundEffect>("win_screen_sound");
                winScreenSound.Play();
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex player;
            if(_exit.Occurred(input, null, out player))
            {
                ScreenManager.RemoveScreen(this);
            }
            if(_replay.Occurred(input, null, out player))
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new GameplayScreen(), null);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var font = ScreenManager.Font;
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(font, "You Win!", new Vector2(310, 175), Color.Gold);
            ScreenManager.SpriteBatch.DrawString(font, "Press Enter to play again", new Vector2(150, 270), Color.DarkGray);
            ScreenManager.SpriteBatch.DrawString(font, "Press ESC to return to menu", new Vector2(120, 320), Color.DarkGray);
            ScreenManager.SpriteBatch.End();
        }
    }
}
