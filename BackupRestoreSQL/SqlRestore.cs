using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackupRestoreSQL
{
    public class SqlRestore
    {
        private Settings _settings = null;
        private LogWriter _logWriter;
        private Telegram _telegram;
        private string _Filepath;
        public SqlRestore(Settings settings, LogWriter logWriter, Telegram telegram, string Filepath)
        {
            _settings = settings;
            _logWriter = logWriter;
            _telegram = telegram;
            _Filepath = Filepath;
        }
        public void sqlRestoreGET()
        {
            Console.WriteLine("Restore.... ");
            if (!String.IsNullOrEmpty(_Filepath) || _settings != null)
            {
                Console.WriteLine("!String.IsNullOrEmpty(_Filepath) || _settings != null ");
                SqlConnection con = new SqlConnection(_settings.connetionStringSetBackup);
                Console.WriteLine("Делаем востановления бекапа " + _settings.DatabaseName);
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                    Console.WriteLine("Соединение установлено  ожидайте.... ");
                    try
                    {

                        string sqlStmt2 = string.Format("ALTER DATABASE [" + _settings.DatabaseName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                        SqlCommand bu2 = new SqlCommand(sqlStmt2, con);
                        bu2.CommandTimeout = 0;
                        bu2.ExecuteNonQuery();

                        _logWriter.LogWrite($" Info:  База переведена в однопользувательский режим");
                        Console.WriteLine("База переведена в однопользувательский режим");

                        string sqlStmt3 = "USE MASTER RESTORE DATABASE [" + _settings.DatabaseName + "] FROM DISK='" + _Filepath + "'WITH REPLACE;";
                        SqlCommand bu3 = new SqlCommand(sqlStmt3, con);
                        bu3.CommandTimeout = 0;
                        _logWriter.LogWrite($" Info:  Отправлен запрос RestoreDataBase ");
                        Console.WriteLine("Отправлен запрос RestoreDataBase");
                        bu3.ExecuteNonQuery();

                        string sqlStmt4 = string.Format("ALTER DATABASE [" + _settings.DatabaseName + "] SET MULTI_USER");

                        SqlCommand bu4 = new SqlCommand(sqlStmt4, con);
                        bu4.CommandTimeout = 0;
                        bu4.ExecuteNonQuery();

                        _logWriter.LogWrite($" Info:  База переведена в многопользувательский режим ");
                        Console.WriteLine("database restoration done successefully");
                        con.Close();
                        Thread.Sleep(120);
                        if (File.Exists(_Filepath))
                        {
                            File.Delete(_Filepath);
                            _logWriter.LogWrite($" Info: Filepath Delete " + _Filepath + " done successefully ");
                            Console.WriteLine("Filepath Delete " + _Filepath + " done successefully");
                        }
                        else
                        {
                            _logWriter.LogWrite($" Error: Filepath Delete " + _Filepath + " не найден!! ");
                            Console.WriteLine("Filepath Delete " + _Filepath + " done successefully");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logWriter.LogWrite($" ERROR:  {ex.ToString()}");
                        if (_settings.TelegramIsNeed == true && !String.IsNullOrEmpty(_settings.TelegramToken) && !String.IsNullOrEmpty(_settings.TelegramChtid))
                        {

                            _telegram.SendMessageAsync($"Company : {_settings.CompanyName} \n  Date: {DateTime.Now}   ERROR:  {ex.ToString()} ").Wait();
                        }
                        Console.WriteLine(ex.ToString());
                        // Environment.Exit(0);
                    }
                }

            }
            else
            {
                Console.WriteLine(" else restore >> String.IsNullOrEmpty(_Filepath) || _settings = null ");
            }

        }
    }
}

