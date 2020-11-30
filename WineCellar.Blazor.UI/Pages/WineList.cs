using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.Blazor.UI.Services;
using WineCellar.Blazor.Shared.Models;
using System;
using Microsoft.Extensions.Logging;
//using Blazorise.DataGrid;
//using Blazorise;

namespace WineCellar.Blazor.UI.Pages
{
    public partial class WineList: ComponentBase
    {
        [Inject]
        public ILogger<Wine> Logger { get; set; }
        
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public IEnumerable<Wine> Wines { get; set; }

        [Inject]
        public IWineDataService WineDataService { get; set; }

        // Demonstrates how a parent component can supply parameters
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int Count { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Wines = (await WineDataService.GetAllWinesAsync()).ToList();
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
