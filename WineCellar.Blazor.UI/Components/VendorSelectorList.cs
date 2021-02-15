using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.UI.Components
{
    public  partial class VendorSelectorList : ComponentBase
    {
        [Inject]
        public IVendorDataService VendorDataService { get; set; }

        private IEnumerable<Vendor> Vendors;

        protected override async Task OnInitializedAsync()
        {
            Vendors = await VendorDataService.GetVendorsAsync();
        }



    }
}
