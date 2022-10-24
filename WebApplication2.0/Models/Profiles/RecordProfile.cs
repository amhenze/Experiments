using AutoMapper;
using WebApplication2._0.Entities;

namespace WebApplication2._0.Models.Profiles
{
    public class RecordProfile : Profile
    {
        public RecordProfile()
        {
            CreateMap<RecordModel, RecordEntity>()
                .ForMember(dest => 
                    dest.RecordId,
                    opt => opt.MapFrom(src => src.RecordId))
                .ForMember(dest =>
                    dest.CollectionId,
                    opt => opt.MapFrom(src => src.CollectionId))
                .ForMember(dest =>
                    dest.Number,
                    opt => opt.MapFrom(src => src.Number))
                .ForMember(dest =>
                    dest.Letter,
                    opt => opt.MapFrom(src => src.Letter))
                .ReverseMap();
        }
    }
}
