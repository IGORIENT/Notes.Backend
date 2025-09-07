using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;

namespace Notes.Persistence
{


    //Для чего всё это нужно

    //Чтобы твой код в контроллерах и сервисах не знал, какую именно базу и как подключать.

    //Чтобы можно было легко заменить SQLite, например, на PostgreSQL — поменял одну строчку в конфигурации, а остальной код не трогаешь.

    //Чтобы код был чистым, удобным для тестирования и поддерживаемым.
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            //"DbConnection" - это ключ в файле конфигурации. Его значение м.б. "Data Source=notes.db"
            // теперь в connectionString лежит НАЗВАНИЕ БАЗЫ ДАННЫХ
            var connectionString = configuration["DbConnection"];
            

            services.AddDbContext<NotesDbContext>(options => //Зарегистрировали NotesDbContext в DI-контейнере.
            {
                options.UseSqlite(connectionString); //нужно использовать SQLite и строку подключения.
            });

            //тут описано: "если попросят интерфейс INotesDbContext - отдай NotesDbContext"
            // <> - какой интерфейс или класс мы регистрируем в контейнер.
            //То есть, когда кто-то попросит INotesDbContext, контейнер должен знать, что ему отдать.

            //Мы говорим DI-контейнеру: «Если кто-то просит INotesDbContext,
            //отдай ему тот же объект, который ты создаёшь для NotesDbContext».
            services.AddScoped<INotesDbContext>(provider => provider.GetService<NotesDbContext>());
            return services;
            
        }
    }
}
