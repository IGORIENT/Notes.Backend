using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using FluentValidation;
using Notes.Application.Common.Behaviors;


namespace Notes.Application
{
    public static class DependencyInjection
    {
        //метод расширение для builder.Services
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                //Это находит и регистрирует все IRequestHandler<,> в текущей сборке.
                //Без этого Send не найдёт обработчики.
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());     
            });



            //Благодаря пакету FluentValidation.DependencyInjectionExtensions
            //эта строка пробежится по сборке и зарегистрирует каждый класс, реализующий IValidator<T>. Это как раз мои валидаторы
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });


            //Подключение поведения-валидации в конвейер MediatR
            
            services.AddTransient(  //создает новый объект каждый раз когда он нужен (нужен на момент вызова команды/запроса)
                typeof(IPipelineBehavior<,>),  //ключ - что могут спросить. Например: IPipelineBehavior<CreateNoteCommand, Unit>
                                               //или IPipelineBehavior<GetNoteListQuery, NoteListVm>

                typeof(ValidationBehavior<,>) //это ответ - что именно отдавать, если спросят ключ.
                                              //Если медиатор захочет IPipelineBehavior<CreateNoteCommand, Unit>
                                              //-DI создаст ValidationBehavior<createNoteCommand, Unit>
                );

            //MediatR, когда обрабатывает команду/запрос, собирает конвейер behaviors.
            //Для этого он спрашивает у DI: «Есть ли зарегистрированные IPipelineBehavior<TRequest, TResponse>?»
            //DI смотрит на эту регистрацию и говорит:
            //«Да, есть — это ValidationBehavior<TRequest, TResponse>».
            //Контейнер создаёт экземпляр ValidationBehavior (каждый раз новый, потому что Transient).
            //Этот экземпляр встраивается в конвейер перед хэндлером.


            return services;
        }

    }
}
