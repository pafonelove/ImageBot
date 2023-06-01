/*
 * 1)	На сервере расположена директория, содержащая графические файлы. Клиент запрашивает оглавление директории, выбирает файл.
 * Сервер отправляет файл клиенту, клиент отображает файл.
 */

namespace ImageBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Построение веб-приложение ASP.NET.
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Базовая конструкция для добавления NewtonsoftJson.
            builder.Services.AddControllers().AddNewtonsoftJson();  // требуется для считывания сообщений, присылаемых ботом

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();  // запуск веб-приложения
        }
    }
}