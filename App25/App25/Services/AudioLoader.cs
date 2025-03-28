using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App25.Views;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App25.Services
{

    public class AudioLoader
    {
        private static AudioLoader _instance;
        public ISimpleAudioPlayer player;
        public bool NonGamePageNavigation { get; set; } = false;

        public static AudioLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AudioLoader();
                }
                return _instance;
            }
        }

        public AudioLoader()
        {
            player = CrossSimpleAudioPlayer.Current;
            player.Loop = true;

        }

        public void LoadAudio(Type type)
        {

            string audioFile = type == typeof(GamePage)
                      ? "App25.assets.audio.themeMusic.metal.mp3"
                      : "App25.assets.audio.themeMusic.miitopiaost.mp3";

            if (player == null)
            {
                player = CrossSimpleAudioPlayer.Current;
                player.Loop = true;
            }

            var assembly = typeof(AudioLoader).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(audioFile);
            if (stream == null)
            {
                Console.WriteLine($"Error: Audio file {audioFile} not found in embedded resources.");
                return;
            }
            player.Load(stream);
        }

        public void Play()
        {
            player.Loop = true;
            player.Play();

        }
        public void Stop() => player.Stop();
        public void Pause()
        {
            player.Pause();
        }

        public void SetVolume(double volume) => player.Volume = volume;
    }
}
