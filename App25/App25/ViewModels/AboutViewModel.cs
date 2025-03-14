using App25.Data;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App25.ViewModels
{
    public class AboutViewModel
    {
        private DatabaseHelper _dbHelper;
        public AboutViewModel()
        {
            _dbHelper = new DatabaseHelper();
        }

        public async Task UpdateVol(double musicVol, double effectVol)
        {
            if (CurrentUser.User != null)
            {
                await _dbHelper.UserVolumeUpdate(CurrentUser.User.Username, musicVol, effectVol);

                CurrentUser.User.Music = musicVol;
                CurrentUser.User.SoundEffectsVol = effectVol;

                Console.WriteLine("Volumes Updated");
            }
        }
    }
}