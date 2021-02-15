using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.UI.Components
{
    public partial class WineRow : ComponentBase
    {
        [Parameter]
        public Wine Wine { get; set; }

        public IWineDataService WineDataService { get; set; }

    }
}
