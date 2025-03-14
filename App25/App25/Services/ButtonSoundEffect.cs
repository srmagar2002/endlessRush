using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace App25.Services
{
    public class ButtonSoundEffect
    {
        private static ButtonSoundEffect instance;
        private ISimpleAudioPlayer _buttonAudio;

        public static ButtonSoundEffect Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ButtonSoundEffect();
                }
                return instance;
            }
        }
        public ButtonSoundEffect()
        {
            LoadButtonAudio();
        }

        private void LoadButtonAudio()
        {
            string soundPath = "App25.assets.audio.effects.buttonclick.mp3";
            _buttonAudio = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer(); // New instance for button sounds

            var assembly = typeof(ButtonSoundEffect).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(soundPath);

            if (stream == null)
            {
                Console.WriteLine($"Error: Button Audio file {soundPath} not found.");
                return;
            }

            _buttonAudio.Load(stream);
        }

        public void Play()
        {
            _buttonAudio?.Play();
        }

        public void SetVolume(double volume)
        {
            if (_buttonAudio != null)
                _buttonAudio.Volume = volume;
        }
    }
}
