using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.UI.Pages
{
    public partial class VendorList : ComponentBase
    {
        [Inject]
        public ILogger<Vendor> Logger { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public IEnumerable<Vendor> Vendors { get; set; }

        [Inject]
        public IVendorDataService VendorDataService { get; set; }

        [Parameter]
        public int Count { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Vendors = (await VendorDataService.GetAllVendorsAsync()).ToList();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e, e.Message);
            }


            if (Count != 0)
            {
                Vendors = Vendors.Take(Count).ToList();
            }
        }
    }
}
