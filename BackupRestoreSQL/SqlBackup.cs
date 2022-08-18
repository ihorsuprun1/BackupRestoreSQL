using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupRestoreSQL
{
    public class SqlBackup
    {
        private Settings _settings = null;
        private LogWriter _logWriter;
        private Telegram _telegram;
        public SqlBackup(Settings settings, LogWriter logWriter, Telegram telegram)
        {
            _settings = settings;
            _logWriter = logWriter;
            _telegram = telegram;
        }
        public string sqlBackupGET()
        {
            if (_settings != null)
            {
                DateTime datatime = DateTime.Now;
                SqlConnection con = new SqlConnection(_settings.connetionStringGetBackup);
               
                string Filepath = _settings.ShareFilepath + _settings.DatabaseName + datatime.ToString().Replace(".", "").Replace(":", "").Replace(" ", "") + ".bak";
                string QuerySql = @"backup database [" + _settings.DatabaseName + "] to disk = '" + Filepath + "'";
                string Result = "";
                using (SqlCommand command = new SqlCommand(QuerySql, con))
                {
                    Console.WriteLine("Делаем  бекапа " + _settings.DatabaseName);
                    if (con.State != ConnectionState.Open)
                    {
                        try
                        {
                            con.Open();
                        }
                        catch (Exception ex)
                        {
                            _logWriter.LogWrite($" ERROR:  {ex.ToString()}");
                            if (_settings.TelegramIsNeed == true && !String.IsNullOrEmpty(_settings.TelegramToken) && !String.IsNullOrEmpty(_settings.TelegramChtid))
                            {

                                _telegram.SendMessageAsync($"Company : {_settings.CompanyName} \n  Date: {DateTime.Now}   ERROR:  {ex.ToString()} ").Wait();
                            }
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    try
                    {
                        command.CommandTimeout = 0;
                        _logWriter.LogWrite("Соединение установлено  ожидайте.... ");
                        Console.WriteLine("Соединение установлено  ожидайте.... ");
                        command.ExecuteNonQuery();
                        con.Close();
                        Console.WriteLine("backup successefull" + _settings.DatabaseName);
                        Result = Filepath;
                    }
                    catch (Exception ex)
                    {
                        _logWriter.LogWrite($" ERROR:  {ex.ToString()}");
                        if (_settings.TelegramIsNeed == true && !String.IsNullOrEmpty(_settings.TelegramToken) && !String.IsNullOrEmpty(_settings.TelegramChtid))
                        {

                            _telegram.SendMessageAsync($"Company : {_settings.CompanyName} \n  Date: {DateTime.Now}   ERROR:  {ex.ToString()} ").Wait();
                        }
                        Console.WriteLine(ex.ToString());

                    }
                }
                Console.WriteLine(Result);
                return Result;
            }
            else
            {
                return null;

            }
        }

    }
}
