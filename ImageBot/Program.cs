/*
 * 1)	�� ������� ����������� ����������, ���������� ����������� �����. ������ ����������� ���������� ����������, �������� ����.
 * ������ ���������� ���� �������, ������ ���������� ����.
 */

namespace ImageBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ���������� ���-���������� ASP.NET.
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // ������� ����������� ��� ���������� NewtonsoftJson.
            builder.Services.AddControllers().AddNewtonsoftJson();  // ��������� ��� ���������� ���������, ����������� �����

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();  // ������ ���-����������
        }
    }
}