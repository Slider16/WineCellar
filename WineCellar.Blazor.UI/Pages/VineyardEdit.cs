using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Components;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.UI.Pages
{
    public partial class VineyardEdit : ComponentBase
    {
        public Vineyard Vineyard { get; set; }

        [Parameter]
        public string VineyardId { get; set; }

        protected string Message { get; set; }

        protected bool Saved;

        protected string StatusClass = string.Empty;

        protected ConfirmBase DeleteConfirmation { get; set; }

        protected string WebsiteUrl = string.Empty;

        [Inject]
        public IVineyardDataService VineyardDataService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Vineyard = new Vineyard();

            if (!string.IsNullOrEmpty(VineyardId))
            {
                Vineyard = await VineyardDataService.GetVineyardByIdAsync(VineyardId).ConfigureAwait(false);                
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (!string.IsNullOrEmpty(VineyardId))
            {
                await VineyardDataService.UpdateVineyardAsync(Vineyard).ConfigureAwait(false);
                StatusClass = "alert-success";
                Message = "Vineyard updated successfully.";
                Saved = true;
            }
            else
            {
                var result = await VineyardDataService.AddVineyardAsync(Vineyard).ConfigureAwait(false);

                if (result != null)
                {
                    StatusClass = "alert-success";
                    Message = "New vineyard added successfully.";
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

        protected void Delete_Click()
        {
            DeleteConfirmation.Show();
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                await VineyardDataService.DeleteVineyard(VineyardId).ConfigureAwait(false);

                StatusClass = "alert-success";
                Message = "Vineyard deleted successfully.";

                Saved = true;
            }
        }

        protected void NavigateToVineyardList()
        {
            NavigationManager.NavigateTo("/vineyard");
        }
    }
}
