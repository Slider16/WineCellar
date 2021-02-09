using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.UI.Components
{
    public partial class WineCard : ComponentBase
    {
        [Parameter]
        public Wine Wine { get; set; }

        [Parameter]
        public string WineId { get; set; }

        protected string Message { get; set; }

        protected string StatusClass = string.Empty;

        [Parameter]
        public bool ShowFooter { get; set; }

        protected ConfirmBase DeleteConfirmation { get; set; }

        [Inject]
        public IWineDataService WineDataService { get; set; }


        protected void Delete_Click()
        {
            DeleteConfirmation.Show();
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                await WineDataService.DeleteWineAsync(Wine.Id).ConfigureAwait(false);

                StatusClass = "alert-success";
                Message = "Wine deleted successfully.";

            }
        }

    }
}
