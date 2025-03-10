using App25.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App25.ViewModels
{
    public class CustomizeViewModel
    {
        private readonly DatabaseHelper _dbHelper;

        public CustomizeViewModel()
        {
            _dbHelper = new DatabaseHelper();
        }
        public async Task UpdateUserAssets(string bgcode, string charcode, string obscode)
        {
            if (CurrentUser.User != null)
            {
                await _dbHelper.UpdateUserCustomization(CurrentUser.User.Username, bgcode, charcode, obscode);

                CurrentUser.User.BackgroundAsset = bgcode;
                CurrentUser.User.CharacterAsset = charcode;
                CurrentUser.User.ObstacleAsset = obscode;

                Console.WriteLine("Assets Updated");
            }
        }

    }
}
