using System;
using System.Collections.Generic;
using System.Text;
using Archer.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Archer.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        private ContentManager content;
        private Song backgroundSong;

        public override void Activate()
        {
            base.Activate();
            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundSong = content.Load<Song>("haunting_music");
            MediaPlayer.Play(backgroundSong);
        }

        public MainMenuScreen() : base("Archer")
        {
            var playGameMenuEntry = new MenuEntry("Play");
            var optionsMenuEntry = new MenuEntry("Options");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (!otherScreenHasFocus && !coveredByOtherScreen && MediaPlayer.State != MediaState.Playing) MediaPlayer.Play(backgroundSong);
        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Stop();
            ScreenManager.AddScreen(new GameplayScreen(), null);
            ScreenManager.AddScreen(new ControlsScreen(), null);
        }

        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SoundOptionsScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
        }
    }
}
