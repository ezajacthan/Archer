using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Archer.StateManagement;
using Archer.Particles;

namespace Archer.Screens
{
    public class ControlsScreen : GameScreen
    {
        private ContentManager contentManager;
        private InputAction _exit;


        public ControlsScreen()
        {
            _exit = new InputAction(new Buttons[] { Buttons.B }, new Keys[] { Keys.Escape, Keys.LeftShift, Keys.RightShift, Keys.Right, Keys.Up, Keys.Down, 
            Keys.Left, Keys.Space}, true);
        }

        public override void Activate()
        {
            if (contentManager == null)
            {
                contentManager = new ContentManager(ScreenManager.Game.Services, "Content");
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex player;
            if (_exit.Occurred(input, null, out player))
            {
                ScreenManager.RemoveScreen(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Gray);

            var font = ScreenManager.Font;
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(font, "SURVIVE ... IF YOU CAN", new Vector2(100, 80), Color.Firebrick);
            ScreenManager.SpriteBatch.DrawString(font, "Arrow Keys to Move", new Vector2(190, 170), Color.WhiteSmoke);
            ScreenManager.SpriteBatch.DrawString(font, "Shift to Run", new Vector2(250, 230), Color.WhiteSmoke);
            ScreenManager.SpriteBatch.DrawString(font, "Space to Shoot", new Vector2(230, 290), Color.WhiteSmoke);
            ScreenManager.SpriteBatch.DrawString(font, "Press any key to start", new Vector2(170, 350), Color.Firebrick);
            ScreenManager.SpriteBatch.End();
        }
    }
}
