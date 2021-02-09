using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.API.AutoMapperProfiles
{
    public class VineyardProfile : Profile
    {
        public VineyardProfile()
        {
            CreateMap<Entities.Vineyard, Models.VineyardDto>();

            CreateMap<Models.VineyardForCreationDto, Entities.Vineyard>();

            CreateMap<Models.VineyardForUpdateDto, Entities.Vineyard>();
        }
    }
}
