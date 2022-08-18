using System;

namespace BackupRestoreSQL
{
    class Program
    {

        private static Settings settings { get; set; }
        static void Main(string[] args)
        {
          
            LogWriter log = new LogWriter();
            JsonService json = new JsonService();
            settings = json.ReadJsonConf();
            Telegram telegram = new Telegram(settings.TelegramToken, settings.TelegramChtid);

            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start();
            SqlBackup sqlBackup = new SqlBackup(settings, log, telegram);
            string filePath = sqlBackup.sqlBackupGET();

            SqlRestore sqlRestore = new SqlRestore(settings, log, telegram, filePath);
            sqlRestore.sqlRestoreGET();

            myStopwatch.Stop();
            TimeSpan ts = myStopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            log.LogWrite("App RunTime: " + elapsedTime);

        }


    }
}
