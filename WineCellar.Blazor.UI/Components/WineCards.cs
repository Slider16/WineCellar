using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.UI.Components
{
    public partial class WineCards : ComponentBase
    {
        [Parameter]
        public IEnumerable<Wine> WineCardWines { get; set; }

        public bool ShowFooter { get; set; } = true;


        //[Inject]
        //public IWineDataService WineDataService { get; set; }

        //protected override async Task OnInitializedAsync()
        //{            
        //    //Wines = (await WineDataService.GetWinesAsync()).ToList();
        //}

    }
}
