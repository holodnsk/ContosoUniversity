using ContosoUniversity.Models;                   // SchoolContext
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;   // CreateScope
using Microsoft.Extensions.Logging;
using System;

namespace ContosoUniversity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<SchoolContext>();
                    DbInitializer.Initialize(context);
                    
                    // EnsureCreated позволяет проверить существование базы данных для контекста.
                    // Если контекст существует, никаких действий не предпринимается.
                    // Если контекст не существует, создаются база данных и вся ее схема.
                    // EnsureCreated не использует миграции для создания базы данных.
                    // Созданную с помощью EnsureCreated базу данных впоследствии нельзя обновить, используя миграции.
                    // EnsureCreated вызывается при запуске приложения, что обеспечивает следующий рабочий процесс:
                    // Удалите базу данных.
                    // Измените схему базы данных(например, добавьте поле EmailAddress).
                    // Запустите приложение.
                    // EnsureCreated создает базу данных со столбцом EmailAddress.
                    // EnsureCreated удобно использовать на ранних стадиях разработки, когда схема часто меняется.
                    // Далее в этом учебнике база данных удаляется и используются миграции.
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}