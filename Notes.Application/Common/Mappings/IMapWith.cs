using AutoMapper;

namespace Notes.Application.Common.Mappings
{
    //переводит исходный тип в нужный тип
    public interface IMapWith <T>
    {
        void Mapping(Profile profile) => 
            profile.CreateMap(typeof(T), GetType());
    }
}
