using AutoMapper;
using WebApplication2._0.Entities;

namespace WebApplication2._0.Models.Profiles
{
    public class CollectiomProfile : Profile
    {
        public CollectiomProfile()
        {
            CreateMap<CollectionModel, CollectionEntity>()
                .ForMember(dest =>
                    dest.CollectionId,
                    opt => opt.MapFrom(src => src.CollectionId))
                .ForMember(dest =>
                    dest.CollectionName,
                    opt => opt.MapFrom(src => src.CollectionName));
        }
    }
}
