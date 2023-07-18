using Telegram.Bot;

namespace ImageBot
{
    public class Bot
    {
        private static TelegramBotClient client { get; set; }

        // Генерация бота через его универсальный токен Telegram.
        public static TelegramBotClient GetTelegramBot()
        {
            if (client != null)
            {
                return client;
            }
            client = new TelegramBotClient("{bot-token}");
            return client;
        }
    }
}
