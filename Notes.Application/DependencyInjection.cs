using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;

namespace Notes.Application
{
    public static class DependencyInjection
    {
        //метод расширение для Services
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
            return services;
        }

    }
}
