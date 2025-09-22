
using AutoMapper;
using System.Reflection;


namespace Notes.Application.Common.Mappings
{
    public class AssemblyMappingProfile : Profile 
    {
        public AssemblyMappingProfile(Assembly assembly) => ApplyMappingsFromAssembly(assembly); // в конструктор

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes() //берет из сборки все public классы
                .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapWith<>))) // собирает все найденные типы в List<IMapWith>
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });

            }
        }
    }
}
