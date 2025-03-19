using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App25.Models
{
    public class Users
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique, NotNull]
        public string Username { get; set; }

        [NotNull]
        public string PasswordHash { get; set; }

        [NotNull]
        public int HighestScore { get; set; } = 0;

        [NotNull]
        public string CharacterAsset { get; set; } = "char1";

        [NotNull]
        public string BackgroundAsset { get; set; } = "bg1";

        [NotNull]
        public string ObstacleAsset { get; set; } = "obstacle1";

        [NotNull]
        public double SoundEffectsVol { get; set; } = 0.5;

        [NotNull]
        public double Music { get; set; } = 0.5;
    }
}
