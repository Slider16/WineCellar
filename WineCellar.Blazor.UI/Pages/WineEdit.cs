using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Components;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.UI.Pages
{
    public partial class WineEdit : ComponentBase
    {
        public Wine Wine { get; set; }

        [Parameter]
        public string WineId { get; set; }
        
        protected string Message { get; set; }       
        
        protected bool Saved;

        protected string StatusClass = string.Empty;

        protected ConfirmBase DeleteConfirmation { get; set; }

        protected ConfirmBase DeleteImageConfirmation { get; set; }

        protected WinePurchaseComponent WinePurchaseComponent { get; set; }
        
        protected string winePurchaseIdToDelete { get; set; }

        protected DisplayImage DisplayImage { get; set; }

        [Inject]
        public IWineDataService WineDataService { get; set; }

        [Inject]
        public IWinePurchaseDataService WinePurchaseDataService { get; set; }

        [Inject]
        public IVineyardDataService VineyardDataService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public List<Vineyard> Vineyards { get; set; } = new List<Vineyard>();

        protected override async Task OnInitializedAsync()
        {                       
            Wine = new Wine();

            Vineyards = (await VineyardDataService.GetVineyardsAsync()).ToList();


            if (!string.IsNullOrEmpty(WineId))
            {      
                Wine = await WineDataService.GetWineByIdAsync(WineId).ConfigureAwait(false);

                if (Wine?.ImageFile?.Content == null)
                    return;

                DisplayImage = new DisplayImage
                {
                    Base64data = Convert.ToBase64String(Wine.ImageFile.Content),
                    ContentType = Wine.ImageFile.ContentType,
                    FileName = Wine.ImageFile.FileName,
                    FileSize = Wine.ImageFile.FileSize
                };
            }
        }


        protected async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var resizedFile = await e.File.RequestImageFileAsync(e.File.ContentType, 640, 480); // resize the image file
            var fileAsByteArray = new byte[resizedFile.Size];
            using (var stream = resizedFile.OpenReadStream())
            {
                await stream.ReadAsync(fileAsByteArray);
            }
            
            Wine.ImageFile = new ImageFile {
                Content = fileAsByteArray,
                ContentType = "jpg",
                FileName = e.File?.Name,
                FileSize = e.File.Size
            };

            DisplayImage = new DisplayImage
            {
                Base64data = Convert.ToBase64String(Wine.ImageFile?.Content),
                ContentType = Wine.ImageFile.ContentType,
                FileName = Wine.ImageFile.FileName,
                FileSize = Wine.ImageFile.FileSize
            };
        }
        
        protected async Task HandleValidSubmitAsync()
        {
            if (!string.IsNullOrEmpty(WineId))
            {
                await WineDataService.UpdateWineAsync(Wine).ConfigureAwait(false);
                StatusClass = "alert-success";
                Message = "Wine updated successfully!";
                Saved = true;
            }
            else
            {
                //Wine.ContentImage = File.ReadAllBytes(@"/images/wine-bottles.png");
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
                    Message = "An error has occurred";
                    Saved = false;
                }
            }
        }

        protected async Task WinePurchaseAdded()
        {
            Wine = await WineDataService.GetWineByIdAsync(WineId).ConfigureAwait(false);            
        }

        protected async Task WinePurchaseEdited()
        {
            Wine = await WineDataService.GetWineByIdAsync(WineId).ConfigureAwait(false);
        }


        protected void QuickAddWinePurchase()
        {
            WinePurchaseComponent.Show();
        }


        protected async Task WinePurchaseDeleted()
        {
            Wine = await WineDataService.GetWineByIdAsync(WineId).ConfigureAwait(false);
        }

        protected async Task ConfirmDeleteImage_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                Wine.ImageFile = null;
                await HandleValidSubmitAsync().ConfigureAwait(false);
            }
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed) 
            {
               
                await WineDataService.DeleteWineAsync(WineId).ConfigureAwait(false);

                StatusClass = "alert-success";
                Message = "Wine deleted successfully.";
                Saved = true;
            }           
        }

        protected void NavigateToWineList()
        {
            NavigationManager.NavigateTo("/wine");            
        }
    }
}
