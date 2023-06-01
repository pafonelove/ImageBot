using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImageBot.Controllers
{

    // Создание контроллера для взаимодействия бота с сервером.
    [ApiController]
    [Route("/")]
    public class BotController : ControllerBase
    {
        private TelegramBotClient bot = Bot.GetTelegramBot();

        [HttpPost]
        public async void Post(Update update)   // вся логика бота работает асинхронно
        {
            // Логи отправляются в консоль.
            long chatId = update.Message.Chat.Id;   // получение Id чата, куда отправлять сообщения бота

            var message = update.Message;   // объект Сообщение, которое отправляет пользователь
            Message sentMessage = new ();   // объект Сообщение, которое присылает бот

            if (message.Text != null)
            {
                // Chat log.
                Console.WriteLine($"[{DateTime.Now}] {message.Chat.FirstName} {message.Chat.LastName} write {message.Text}");

                // В зависимости от отправленного Клиентом (пользователем) сообщения Сервер (бот) отвечает заданной конструкцией.
                switch (message.Text)
                {
                    case @"/start":
                        await bot.SendTextMessageAsync(message.Chat.Id, "Выберите действие", replyMarkup: GetButtons());    // запуск бота
                        break;
                    case "Поздороваться":
                        await bot.SendTextMessageAsync(chatId, $"Приветствую тебя, {message.Chat.FirstName} {message.Chat.LastName}!", replyMarkup: GetButtons());  // приветствие бота (аналог пинга)
                        break;
                    case "Перейти на канал разработчика":
                        await bot.SendTextMessageAsync(chatId, @"Ссылка для перехода на канал: @bearded_brain", replyMarkup: GetButtons()); // сообщение-ссылка на Tg-канал разработчика бота
                        break;
                    case "Узнать id переписки":
                        await bot.SendTextMessageAsync(chatId, $"Id чата: {chatId}", replyMarkup: GetButtons());    // отображение Id чата
                        break;
                    case "Подключиться к серверу изображений":
                        
                        ChooseDir(update, chatId, sentMessage); // отображение списка директорий на сервере                
                        break;

                    case "Директория 1 - Cars":
                        ChooseFile(update, chatId, sentMessage, "Cars");    // отображение файлов в директории "Cars"
                        break;

                        case "Car1.jpg":
                            await bot.SendPhotoAsync(chatId, InputFile.FromUri("https://paul.l27001.net/Cars/Car1.jpg"), caption: "Машина 1", parseMode: ParseMode.Html);   // отправление картинки ботом в чат
                            break;
                        case "Car2.jpg":
                            await bot.SendPhotoAsync(chatId, InputFile.FromUri("https://paul.l27001.net/Cars/Car2.jpg"), caption: "Машина 2", parseMode: ParseMode.Html);
                            break;
                        case "Car3.jpg":
                            await bot.SendPhotoAsync(chatId, InputFile.FromUri("https://paul.l27001.net/Cars/Car3.jpg"), caption: "Машина 3", parseMode: ParseMode.Html);
                            break;

                    case "Директория 2 - Kittens":
                        ChooseFile(update, chatId, sentMessage, "Kittens");    // отображение файлов в директории "Kittens"
                        break;

                        case "Kitten1.jpg":
                            await bot.SendPhotoAsync(chatId, InputFile.FromUri("https://paul.l27001.net/Kittens/Kitten1.jpg"), caption: "Котик 1", parseMode: ParseMode.Html);
                            break;
                        case "Kitten2.jpg":
                            await bot.SendPhotoAsync(chatId, InputFile.FromUri("https://paul.l27001.net/Kittens/Kitten2.jpg"), caption: "Котик 2", parseMode: ParseMode.Html);
                            break;
                        case "Kitten3.jpg":
                            await bot.SendPhotoAsync(chatId, InputFile.FromUri("https://paul.l27001.net/Kittens/Kitten3.jpg"), caption: "Котик 3", parseMode: ParseMode.Html);
                            break;

                    case "Директория 3 - Nature":
                        ChooseFile(update, chatId, sentMessage, "Nature");    // отображение файлов в директории "Nature"
                        break;

                        case "Nature1.jpg":
                            await bot.SendPhotoAsync(chatId, InputFile.FromUri("https://paul.l27001.net/Nature/Nature1.jpg"), caption: "Природа 1", parseMode: ParseMode.Html);
                            break;
                        case "Nature2.jpg":
                            await bot.SendPhotoAsync(chatId, InputFile.FromUri("https://paul.l27001.net/Nature/Nature2.jpg"), caption: "Природа 2", parseMode: ParseMode.Html);
                            break;
                        case "Nature3.jpg":
                            await bot.SendPhotoAsync(chatId, InputFile.FromUri("https://paul.l27001.net/Nature/Nature3.jpg"), caption: "Природа 3", parseMode: ParseMode.Html);
                            break;

                    case "Вернуться назад":
                        await bot.SendTextMessageAsync(chatId, "Возврат в главное меню.", replyMarkup: GetButtons());   // возврат в главное меню
                        break;

                    default:
                        await bot.SendTextMessageAsync(chatId, "Выберите корректное действие (нажмите на одну из кнопок)");
                        break;

                }
            }
        }
        [HttpGet]
        public string Get()
        {
            // Сообщение-баннер, которое отображается при подключении по адресу,
            // указанному в ngrok при пробрасывании портов.
            return "BeardedBrain Telegram bot was started";
        }

        // Генерация базовых кнопок при запуске бота.
        private static IReplyMarkup? GetButtons()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] {"Поздороваться", "Перейти на канал разработчика", "Узнать id переписки"},
                new KeyboardButton[] { "Подключиться к серверу изображений" }
            })
            {
                ResizeKeyboard = true
            };
        }

        // Обработчик ошибок Telegram Bot API.
        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            var ErrorMessage = arg2 switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => arg2.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        // Отображение директорий сервера для выбора.
        private async void ChooseDir(Update update, long chatId, Message sentMessage)
        {
            string[] dirs = { "Cars", "Kittens", "Nature" };
            string[] dirsRU = { "Автомобили", "Котики", "Природа" };

            await bot.SendTextMessageAsync(chatId, "Выберите директорию, из которой хотите скачать картинку:", replyMarkup: GetDirButtons());
            for (int i = 0, k = 1; i < dirs.Length; i++, k++)
            {
                await bot.SendTextMessageAsync(chatId, $"{k}. {dirs[i]} ({dirsRU[i]})");
            }

            var message = update.Message;
            Console.WriteLine(message.Text);

            sentMessage.Text = "Нажмите на одну из кнопок";
        }

        // Генерация кнопок выбора директории.
        private static IReplyMarkup? GetDirButtons()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] {"Директория 1 - Cars", "Директория 2 - Kittens", "Директория 3 - Nature"},
                new KeyboardButton[] { "Вернуться назад" }
            })
            {
                ResizeKeyboard = true
            };
        }

        // Выбор картинки в указанной директории.
        private async void ChooseFile(Update update, long chatId, Message sentMessage, string dirName)
        {
            string[] filesCars = { "Car1.jpg", "Car2.jpg", "Car3.jpg" };
            string[] filesKittens = { "Kitten1.jpg", "Kitten2.jpg", "Kitten3.jpg" };
            string[] filesNature = { "Nature1.jpg", "Nature2.jpg", "Nature3.jpg" };

            await bot.SendTextMessageAsync(chatId, "Выберите файл, который хотите скачать:", replyMarkup: GetPictureButtons(dirName));

            if (dirName == "Cars")
            {
                for (int i = 0, k = 1; i < filesCars.Length; i++, k++)
                {
                    await bot.SendTextMessageAsync(chatId, $"{k}. {filesCars[i]}");
                }
            }

            if (dirName == "Kittens")
            {
                for (int i = 0, k = 1; i < filesKittens.Length; i++, k++)
                {
                    await bot.SendTextMessageAsync(chatId, $"{k}. {filesKittens[i]}");
                }
            }

            if (dirName == "Nature")
            {
                for (int i = 0, k = 1; i < filesNature.Length; i++, k++)
                {
                    await bot.SendTextMessageAsync(chatId, $"{k}. {filesNature[i]}");
                }
            }

        }

        // Генерация кнопок выбора картинки в указанной директории.
        private static IReplyMarkup? GetPictureButtons(string dirName)
        {
            if (dirName == "Cars")
            {
                return new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] {"Car1.jpg", "Car2.jpg", "Car3.jpg"},
                    new KeyboardButton[] { "Вернуться назад" }
                })
                {
                    ResizeKeyboard = true
                };
            }

            if (dirName == "Kittens")
            {
                return new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] { "Kitten1.jpg", "Kitten2.jpg", "Kitten3.jpg"},
                    new KeyboardButton[] { "Вернуться назад" }
                })
                {
                    ResizeKeyboard = true
                };
            }

            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "Nature1.jpg", "Nature2.jpg", "Nature3.jpg"},
                new KeyboardButton[] { "Вернуться назад" }
            })
            {
                ResizeKeyboard = true
            };

        }
    }
}
