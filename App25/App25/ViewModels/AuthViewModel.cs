using App25.Data;
using App25.Models;
using App25.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace App25.ViewModels
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseHelper _dbHelper;

        public event PropertyChangedEventHandler PropertyChanged;



        public AuthViewModel()
        {
            _dbHelper = new DatabaseHelper();
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            int result = await _dbHelper.RegisterUser(new Users
            {
                Username = username,
                PasswordHash = SecurityHelper.HashPassword(password)
            });

            return result > 0;
        }

        public async Task<bool> LoginUser(string username, string password)
        {
            var user = await _dbHelper.ValidateUser(username, SecurityHelper.HashPassword(password));
            if (user)
            {
                CurrentUser.User = await _dbHelper.GetUserByUsername(username); // Store the logged-in user
                return true;
            }
            return false;
        }

        public async Task UpdateHighestScore(int newScore)
        {
            if (CurrentUser.User != null)
            {
                if (newScore > CurrentUser.User.HighestScore) // Only update if new score is higher
                {
                    await _dbHelper.UpdateUserScore(CurrentUser.User.Username, newScore);
                    CurrentUser.User.HighestScore = newScore;
                    OnPropertyChanged(nameof(CurrentUser.User));
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}