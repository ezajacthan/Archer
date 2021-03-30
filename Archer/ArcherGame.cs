using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Archer.Screens;
using Archer.StateManagement;

namespace Archer
{
    public class ArcherGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager screenManager;

        public bool ShouldExit;

        public Song MenuMusic { get; set; }

        public Song GameMusic { get; set; }

        public GameScreen CurrentScreen { get; set; }

        public ArcherGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            MainMenuScreen menuScreen = new MainMenuScreen();
            DebugScreen debugScreen = new DebugScreen();
            GameScreen currScreen = menuScreen;
            CurrentScreen = currScreen;
            screenManager.AddScreen(currScreen, null);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
    }
}
