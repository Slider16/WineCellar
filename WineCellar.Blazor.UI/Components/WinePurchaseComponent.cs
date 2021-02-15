using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.UI.Components
{
    public partial class WinePurchaseComponent : ComponentBase
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public Wine Wine { get; set; }
                
        [Parameter]
        public List<WinePurchase> WinePurchases { get; set; }

        [Parameter]
        public EventCallback<WinePurchase> OnWinePurchaseAdded { get; set; }

        [Parameter]
        public EventCallback<WinePurchase> OnWinePurchaseEdited { get; set; }

        [Parameter]
        public EventCallback<WinePurchase> OnWinePurchaseDeleted { get; set; }


        protected string Message { get; set; }

        protected string StatusClass = string.Empty;

        protected bool ShowAlertMessage = false;

        protected bool ShowDialog { get; set; }

        public WinePurchase WinePurchase { get; set; }

        public WinePurchase WinePurchaseToDelete { get; set; }

        protected ConfirmBase DeleteConfirmation { get; set; }

        public string WinePurchaseId { get; set; }

        public string PurchaseDateString { get; set; }

        public List<Vendor> Vendors { get; set; } = new List<Vendor>();
         
        [Inject]
        public IWinePurchaseDataService WinePurchaseDataService { get; set; }

        [Inject]
        public IVendorDataService VendorDataService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            WinePurchase = new WinePurchase();

            Vendors = (await VendorDataService.GetVendorsAsync()).ToList();

            if (!string.IsNullOrEmpty(WinePurchaseId))
            {
                WinePurchase = await WinePurchaseDataService.GetWinePurchaseByIdAsync(WinePurchaseId).ConfigureAwait(false);
            }
        }

        protected async Task HandleValidSubmitAsync(EditContext editContext)
        {
            if (!string.IsNullOrEmpty(WinePurchase.Id))
            {
                await WinePurchaseDataService.UpdateWinePurchaseAsync(WinePurchase).ConfigureAwait(false);
                

                StatusClass = "alert-success";
                Message = "Wine purchase updated successfully.";
                ShowAlertMessage = true;

                await OnWinePurchaseEdited.InvokeAsync(WinePurchase).ConfigureAwait(false);
                
            }
            else
            {
                WinePurchase.WineId = Wine.Id;
                var winePurchase = await WinePurchaseDataService.AddWinePurchaseAsync(WinePurchase).ConfigureAwait(false);
                if (winePurchase == null)
                {
                    StatusClass = "alert-danger";
                    Message = "An error has occurred";
                    ShowAlertMessage = true;
                }

                await OnWinePurchaseAdded.InvokeAsync(WinePurchase).ConfigureAwait(false);
            }

            Close();            
        }

        protected async Task WinePurchaseLink_Click(string winePurchaseId)
        {
            ShowAlertMessage = false;

            var winePurchase = await WinePurchaseDataService.GetWinePurchaseByIdAsync(winePurchaseId).ConfigureAwait(false);
            if (winePurchase == null)
            {
                StatusClass = "alert-danger";
                Message = "An error has occurred";
                ShowAlertMessage = true;
            }

            WinePurchase = winePurchase;
            ShowDialog = true;
        }

        protected async Task<WinePurchase> GetWinePurchaseAsync(string winePurchaseId)
        {
            var winePurchase = await WinePurchaseDataService.GetWinePurchaseByIdAsync(winePurchaseId).ConfigureAwait(false);
            if (winePurchase == null)
            {
                StatusClass = "alert-danger";
                Message = "An error has occurred";
                ShowAlertMessage = true;
            }

            return winePurchase;
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                if (WinePurchaseToDelete != null)
                {
                    await WinePurchaseDataService.DeleteWinePurchaseAsync(WinePurchaseToDelete.Id);

                    await OnWinePurchaseDeleted.InvokeAsync(WinePurchaseToDelete).ConfigureAwait(false);

                    WinePurchaseToDelete = null;
                }
            }
        }

        internal void Show()
        {
            ResetDialog();
            ShowDialog = true;
        }

        private void ResetDialog()
        {
            WinePurchase = new WinePurchase { PurchaseDate = DateTime.Now };
        }

        internal void Close()
        {
            ShowDialog = false;
        }

        protected void Delete_Click(WinePurchase winePurchase)
        {
            WinePurchaseToDelete = winePurchase;

            DeleteConfirmation.Show();
        }

        protected void ClearMessageBanner()
        {
            ShowAlertMessage = false;
            NavigationManager.NavigateTo($"/wineedit/{Wine.Id}");
        }

    }
}
