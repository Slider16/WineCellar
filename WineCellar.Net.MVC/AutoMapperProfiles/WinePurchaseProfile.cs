using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineCellar.Net.MVC.AutoMapperProfiles
{
    public class WinePurchaseProfile : Profile
    {
        public WinePurchaseProfile()
        {
            CreateMap<Data.Entities.WinePurchase, ViewModels.WinePurchaseViewModel>();
        }
    }
}
