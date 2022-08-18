using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BackupRestoreSQL
{
    public class Telegram
    {
        private static string _token;
        private static string _telegramChatId;

        public Telegram(string token, string telegramChatId)
        {
            _token = token;
            _telegramChatId = telegramChatId;

        }

        public async Task SendMessageAsync(string telegramSendMessange)
        {
            try
            {
                TelegramBotClient botClient = new TelegramBotClient(_token);
                var me = botClient.GetMeAsync().Result;
                Console.WriteLine(me.Username);
                var t = await botClient.SendTextMessageAsync(_telegramChatId, telegramSendMessange);
            }
            catch
            {
            }

        }
    }
}

