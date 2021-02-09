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
    public partial class WinePriceAdjust : ComponentBase
    {
        [Inject]
        public ILogger<Wine> Logger { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public IEnumerable<Wine> Wines { get; set; }

        [Inject]
        public IWineDataService WineDataService { get; set; }


        [Parameter]
        public int Count { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Wines = (await WineDataService.GetWinesForPriceAdjustAsync()).ToList();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e, e.Message);
            }


            if (Count != 0)
            {
                Wines = Wines.Take(Count).ToList();
            }
        }

    }
}
