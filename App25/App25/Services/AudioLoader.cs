using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App25.Services
{

    public class AudioLoader
    {
        public ISimpleAudioPlayer player;


        public AudioLoader()
        {
            player = CrossSimpleAudioPlayer.Current;
            player.Loop = true;

        }

        public void LoadAudio(int song)
        {

            string audioFile = "";

            if (song == 1)
            {
                audioFile = "App25.assets.audio.themeMusic.electronic.mp3";
            }
            else if (song == 2)
            {
                audioFile = "App25.assets.audio.themeMusic.metal.mp3";
            }
            else if (song == 3)
            {
                audioFile = "App25.assets.audio.themeMusic.miitopiaost.mp3";
            }

            if (player == null)
            {
                player = CrossSimpleAudioPlayer.Current;
                player.Loop = true;
            }
            var assembly = typeof(App).Assembly;
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

        public void SetVolume(double volume) => player.Volume = volume;


        public async Task FadeOut()
        {
            for (double i = player.Volume; i >= 0; i -= 0.05)
            {
                player.Volume = i;
                await Task.Delay(100);
            }
            player.Stop();
            player.Volume = 1.0;
        }
    }
}
