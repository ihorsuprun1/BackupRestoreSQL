using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupRestoreSQL
{
    public class Settings
    {
        public string CompanyName { get; set; } = "Unknow";
        public bool TelegramIsNeed { get; set; } = true;
        public string TelegramChtid { get; set; } = "";
        public string TelegramToken { get; set; } = "";
        public string connetionStringGetBackup { get; set; } = @"Data Source = 10.0.0.9; Initial Catalog = master; User ID = sa; Password = pass";
        public string connetionStringSetBackup { get; set; } = @"Data Source=10.0.0.10;Initial Catalog = master; User ID = sa; Password = pass";
        public string DatabaseName { get; set; } = @"Database";
        public string ShareFilepath { get; set; } = @"\\10.8.0.8\TempBackup$\";
    }
}
