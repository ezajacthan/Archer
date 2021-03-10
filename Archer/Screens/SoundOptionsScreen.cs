using System;
using System.Collections.Generic;
using System.Text;
using Archer.StateManagement;
using Archer.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Archer.Screens
{
    public class SoundOptionsScreen: MenuScreen
    {
        private enum VolumeOptions
        {
            Off = 0,
            Minimum = 1,
            Quiet = 2,
            Normal = 3,
            Loud = 4,
            Maximum = 5,
        }

        private VolumeOptions effectsVolumeOption = VolumeOptions.Normal;
        private VolumeOptions musicVolumeOption = VolumeOptions.Normal;

        private readonly MenuEntry mainVolumeMenuEntry;
        private readonly MenuEntry musicVolumeMenuEntry;
        private readonly MenuEntry exitMenuEntry;

        public SoundOptionsScreen() : base("Archer")
        {
            mainVolumeMenuEntry = new MenuEntry("");
            musicVolumeMenuEntry = new MenuEntry("");
            exitMenuEntry = new MenuEntry("Back to Main Menu");
            SetMenuEntryText();

            mainVolumeMenuEntry.Selected += EffectsVolumeEntrySelected;
            musicVolumeMenuEntry.Selected += MusicMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(mainVolumeMenuEntry);
            MenuEntries.Add(musicVolumeMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void SetMenuEntryText()
        {
            mainVolumeMenuEntry.Text = $"SFX Volume: {effectsVolumeOption}";
            musicVolumeMenuEntry.Text = $"Music Volume: {musicVolumeOption}";
        }

        private void EffectsVolumeEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            effectsVolumeOption++;
            if (effectsVolumeOption > VolumeOptions.Maximum)
            {
                effectsVolumeOption = 0;
            }
            float volumeOption = (float)effectsVolumeOption;
            float newVolume = (float)volumeOption / 5.0f;
            SoundEffect.MasterVolume = newVolume;
            SetMenuEntryText();
        }

        private void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            musicVolumeOption++;
            if (musicVolumeOption > VolumeOptions.Maximum)
            {
                musicVolumeOption = 0;
            }
            float volumeOption =(float)musicVolumeOption;
            float newVolume = (float)volumeOption / 5.0f;
            MediaPlayer.Volume = newVolume;
            SetMenuEntryText();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.RemoveScreen(this);
        }
    }
}
