
using AutoMapper;
using System.Reflection;


namespace Notes.Application.Common.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly) => ApplyMappingsFromAssembly(assembly); // в конструктор

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {

            // А. нахожу все классы, реализующие IMapWith
            var types = assembly.GetExportedTypes() //берет из сборки все public классы
                .Where(type => type.GetInterfaces().Any(
                    i => i.IsGenericType 
                    && i.GetGenericTypeDefinition() == typeof(IMapWith<>))) // GetGenericTypeDefinition() возвращает Type дженерик интерфейс без конкретного параметра IMapWith<Note>, а IMapWith<>
                                                                            // IMapWith<> без typeof это просто синтаксическое имя интерфейса, но не тип         
                .ToList();


            // Б. вызываю метод Mapping в каждом таком классе
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);  // создает экзмепляр Dto и Vm
                var methodInfo = type.GetMethod("Mapping");     // находит метод "Mapping" в данных классах
                methodInfo?.Invoke(instance, new object[] { this });    // вызывает Mapping данного класса
                
            }
        }
    }
}
