using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.Net.API.AutoMapperProfiles
{
    public class WineProfile : Profile
    {
        public WineProfile()
        {
            CreateMap<Entities.Wine, Models.WineDto>()
                .ForMember(
                    dest => dest.WinePurchases,
                    opt => opt.MapFrom(src => src.WinePurchases));

            CreateMap<Models.WineForCreationDto, Entities.Wine> ();

            CreateMap<Models.WineForUpdateDto, Entities.Wine>();
        }
    }
}
