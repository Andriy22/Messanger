using AutoMapper;

namespace Application.Common.Mappings;

public interface IMapWith<T>
    where T : class
{
    void Mapping(Profile profile) =>
          profile.CreateMap(typeof(T), GetType());
}
