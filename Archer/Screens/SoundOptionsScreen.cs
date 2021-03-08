using System;
using System.Collections.Generic;
using System.Text;
using Archer.StateManagement;
using Archer.Screens;
using Microsoft.Xna.Framework;

namespace Archer.Screens
{
    public class SoundOptionsScreen: MenuScreen
    {
        private enum VolumeOptions
        {
            Off,
            Minimum,
            Quiet,
            Normal,
            Loud,
            Maximum,
        }

        private VolumeOptions masterVolumeOption = VolumeOptions.Normal;
        private VolumeOptions musicVolumeOption = VolumeOptions.Normal;

        private readonly MenuEntry mainVolumeMenuEntry;
        private readonly MenuEntry musicVolumeMenuEntry;
        private readonly MenuEntry exitMenuEntry;

        public SoundOptionsScreen() : base("Archer")
        {
            mainVolumeMenuEntry = new MenuEntry("");
            musicVolumeMenuEntry = new MenuEntry("");
            exitMenuEntry = new MenuEntry("");
            SetMenuEntryText();

            mainVolumeMenuEntry.Selected += MasterVolumeEntrySelected;
            musicVolumeMenuEntry.Selected += MusicMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(mainVolumeMenuEntry);
            MenuEntries.Add(musicVolumeMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void SetMenuEntryText()
        {
            mainVolumeMenuEntry.Text = $"Master Volume: {masterVolumeOption}";
            musicVolumeMenuEntry.Text = $"Master Volume: {musicVolumeOption}";
        }

        private void MasterVolumeEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            masterVolumeOption++;
            if (masterVolumeOption > VolumeOptions.Maximum)
            {
                masterVolumeOption = 0;
            }
            SetMenuEntryText();
        }

        private void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            musicVolumeOption++;
            if (musicVolumeOption > VolumeOptions.Maximum)
            {
                musicVolumeOption = 0;
            }
            SetMenuEntryText();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.RemoveScreen(this);
        }
    }
}
