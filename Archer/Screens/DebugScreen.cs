using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Archer.StateManagement;
using Archer.Sprites;

namespace Archer.Screens
{
    class DebugScreen : GameScreen
    {
        private ContentManager contentManager;
        private Texture2D treeTexture;
        private Texture2D grassTexture;
        private InputAction _exit;
        private SpookyTree1Sprite tree;
        private GrassTexture grass;


        public DebugScreen()
        {
            _exit = new InputAction(new Buttons[] { Buttons.B }, new Keys[] { Keys.Escape }, true);
        }

        public override void Activate()
        {
            if (contentManager == null)
            {
                contentManager = new ContentManager(ScreenManager.Game.Services, "Content");
                treeTexture = contentManager.Load<Texture2D>("spooky_trees");
                grassTexture = contentManager.Load<Texture2D>("ground_grass_gen_08");
                tree = new SpookyTree1Sprite(new Vector2(200, 200), treeTexture);
                grass = new GrassTexture(grassTexture);
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
            var font = ScreenManager.Font;
            ScreenManager.SpriteBatch.Begin();
            grass.Draw(ScreenManager.SpriteBatch);
            tree.Draw(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
        }
    }
}
