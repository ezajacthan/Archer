using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Archer.Sprites
{
    public class GrassTexture
    {
        private Color color = new Color(160, 82, 45, 210);

        /// <summary>
        /// property to hold the texture of the sprite
        /// </summary>
        public Texture2D Texture { get; set; }

        public GrassTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, new Vector2(0,0), null, color, 0, new Vector2(0,0), 2f, SpriteEffects.None, 0);
        }
    }
}
