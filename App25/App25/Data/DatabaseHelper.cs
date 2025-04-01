using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using App25.Models;
using System.Net.Sockets;
using System.Linq;

namespace App25.Data
{

    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper()
        {
            string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "users.db3");
            _database = new SQLiteAsyncConnection(dbpath);
            _database.CreateTableAsync<Users>().Wait();
        }

        public async Task<int> RegisterUser(Users users)
        {
            var existingUser = await GetUserByUsername(users.Username);
            if (existingUser == null)
            {
                return await _database.InsertAsync(users);
            }
            return 0;
        }

        public async Task<Users> GetUserByUsername(string username)
        {
            return await _database.Table<Users>().Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<bool> ValidateUser(string username, string password)
        {
            var user = await GetUserByUsername(username);
            if (user != null && user.PasswordHash == password)
            {
                return true;
            }
            return false;
        }

        public async Task<int> UpdateUserScore(string username, int newScore)
        {
            var user = await GetUserByUsername(username);
            if (user != null && newScore > user.HighestScore)
            {
                user.HighestScore = newScore;
                return await _database.UpdateAsync(user);
            }
            return 0;
        }

        public async Task<int> UpdateUserCustomization(string username, string background, string character, string obstacle)
        {
            var user = await GetUserByUsername(username);
            if (user != null)
            {
                user.BackgroundAsset = background;
                user.CharacterAsset = character;
                user.ObstacleAsset = obstacle;

                return await _database.UpdateAsync(user);

            }
            return 0;
        }

        public async Task<int> UserVolumeUpdate(string username, double musicVol, double effectVol)
        {
            var user = await GetUserByUsername(username);

            if (user != null)
            {

                user.Music = musicVol;
                user.SoundEffectsVol = effectVol;

                return await _database.UpdateAsync(user);
            }

            return 0;
        }

        public async Task<List<UserScoreDTO>> GetUserScores()
        {
            var users = await _database.Table<Users>()
                .OrderByDescending(u => u.HighestScore)
                .Take(5)
                .ToListAsync();

            return users.Select(u => new UserScoreDTO
            {
                Username = u.Username,
                HighestScore = u.HighestScore,
            }).ToList();
        }

        public async Task DeleteUser(string username)
        {
            var user = await GetUserByUsername(username);
            if (user != null)
            {
                await _database.DeleteAsync(user);
            }
        }

        public async Task<int> UpdateUserMusic(string username, string songNo)
        {
            var user = await GetUserByUsername(username);
            if (user != null)
            {
                user.NonGamePageMusic = songNo;
                return await _database.UpdateAsync(user);
            }
            return 0;
        }
    }
}
