using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.UI.Pages
{
    public partial class WineEdit : ComponentBase
    {

        public Wine Wine { get; set; }
        
        [Parameter]
        public string WineId { get; set; }
        protected string Message { get; set; }
        protected string Year { get; set; } = string.Empty;
        
        protected bool Saved;
        
        protected string StatusClass = string.Empty;
        

        [Inject]
        public IWineDataService WineDataService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }


        protected override async Task OnInitializedAsync()
        {           
            Wine = new Wine();
            
            if (!string.IsNullOrEmpty(WineId))
            {
                Wine = await WineDataService.GetWineByIdAsync(WineId).ConfigureAwait(false);

                if (Wine != null)
                {
                    Year = Wine.Year.ToString();
                }
            }
        }
        protected async Task HandleValidSubmit()
        {

            if (!string.IsNullOrEmpty(WineId))
            {
                await WineDataService.UpdateWineAsync(Wine).ConfigureAwait(false);
                StatusClass = "alert-success";
                Message = "Wine updated successfully.";
                Saved = true;
            }
            else
            {
                var result = await WineDataService.AddWineAsync(Wine).ConfigureAwait(false);
                if (result != null)
                {
                    StatusClass = "alert-success";
                    Message = "New wine added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "An error has occured";
                    Saved = false;
                }
            }
        }

        public async Task DeleteWine()
        {
            await WineDataService.DeleteWine(WineId).ConfigureAwait(false);

            StatusClass = "alert-success";
            Message = "Wine deleted successfully.";

            Saved = true;
        }

        protected void NavigateToWineList()
        {
            NavigationManager.NavigateTo("/winelist");
        }
    }
}
