using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.Blazor.UI.Services;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Pages
{
    public partial class WineList: ComponentBase
    {
        public IEnumerable<Wine> Wines { get; set; }

        [Inject]
        public IWineDataService WineDataService { get; set; }

        
        protected override async Task OnInitializedAsync()
        {
            Wines = (await WineDataService.GetAllWinesAsync()).ToList();            
        }
}
}
