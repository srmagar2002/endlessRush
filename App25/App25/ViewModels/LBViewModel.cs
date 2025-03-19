using App25.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App25.ViewModels
{
    public class LBViewModel
    {
        private DatabaseHelper dbHelper;
        public LBViewModel()
        {
            dbHelper = new DatabaseHelper();
        }

        public async Task<List<UserScoreDTO>> getLBData()
        {
            var topusers = await dbHelper.GetUserScores();
            return topusers;
        }
    }
}
