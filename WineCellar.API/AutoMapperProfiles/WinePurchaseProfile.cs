using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.Net.API.AutoMapperProfiles
{
    public class WinePurchaseProfile : Profile
    {
        // This profile is used by the WineProfile during mapping because
        // we are mapping to an actual DTO, (WinePurchaseDto), instead of 
        // just matching the entity name of WinePurchase.
        public WinePurchaseProfile()
        {
            CreateMap<Entities.WinePurchase, Models.WinePurchaseDto>();

            CreateMap<Models.WinePurchaseForCreationDto, Entities.WinePurchase>();
        }
    }
}
