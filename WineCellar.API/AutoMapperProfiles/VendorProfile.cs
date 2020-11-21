using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.Net.API.AutoMapperProfiles
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            CreateMap<Entities.Vendor, Models.VendorDto>();

            CreateMap<Models.VendorForCreationDto, Entities.Vendor>();

            CreateMap<Models.VendorForUpdateDto, Entities.Vendor>();
        }

    }
}
