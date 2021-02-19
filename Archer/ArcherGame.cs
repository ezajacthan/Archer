using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Archer
{
    public class ArcherGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ArcherSprite archerSprite;
        private IceGhostSprite ghostSprite;
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
            ghostSprite = new IceGhostSprite();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            archerSprite.LoadContent(Content);
            ghostSprite.LoadContent(Content);
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
                ghostSprite.Update(gameTime);

                foreach (ArrowSprite arrow in archerSprite.Arrows)
                {
                    if (arrow.Bounds.CollidesWith(ghostSprite.Bounds))
                    {
                        ghostSprite.IsHit = true;
                        archerSprite.didHit = true;
                    }
                }
                if (archerSprite.didHit && archerSprite.Arrows.Count>0)
                {
                    archerSprite.Arrows.Dequeue();
                }
                if (ghostSprite.Fireball != null && ghostSprite.Fireball.Bounds.CollidesWith(archerSprite.Bounds) || (!ghostSprite.IsDead && archerSprite.Bounds.CollidesWith(ghostSprite.Bounds)))
                {
                    isGameOver = true;
                }
                if(ghostSprite.IsDead)
                {
                    isWin = true;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            ghostSprite.Draw(gameTime, _spriteBatch, isGameOver);
            archerSprite.Draw(gameTime, _spriteBatch);
            if (isGameOver) _spriteBatch.Draw(gameOverTexture, new Rectangle(300, 150, 200, 100), Color.White);
            else if (isWin) _spriteBatch.Draw(winTexture, new Rectangle(300, 150, 200, 100), Color.ForestGreen);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
