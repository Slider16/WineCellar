using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;
using WineCellar.Blazor.UI.Components;
using WineCellar.Blazor.UI.Services;
namespace WineCellar.Blazor.UI.Pages
{
    public partial class VendorEdit : ComponentBase
    {
        public Vendor Vendor { get; set; }

        [Parameter]
        public string VendorId { get; set; }

        protected string Message { get; set; }

        protected bool Saved;

        protected string StatusClass = string.Empty;

        protected ConfirmBase DeleteConfirmation { get; set; }

        protected string WebsiteUrl = string.Empty;


        [Inject]
        public IVendorDataService VendorDataService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Vendor = new Vendor();

            if (!string.IsNullOrEmpty(VendorId))
            {
                Vendor = await VendorDataService.GetVendorByIdAsync(VendorId).ConfigureAwait(false);
                WebsiteUrl = Vendor.WebSite?.ToString();
            }
        }


        protected async Task HandleValidSubmit()
        {
            if (!string.IsNullOrEmpty(WebsiteUrl))
            {
                Vendor.WebSite = new Uri(WebsiteUrl);
            }

            if (!string.IsNullOrEmpty(VendorId))
            {
                await VendorDataService.UpdateVendorAsync(Vendor).ConfigureAwait(false);
                StatusClass = "alert-success";
                Message = "Vendor updated successfully.";
                Saved = true;
            }
            else
            {
                var result = await VendorDataService.AddVendorAsync(Vendor).ConfigureAwait(false);

                if (result != null)
                {
                    StatusClass = "alert-success";
                    Message = "New vendor added successfully.";
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
                await VendorDataService.DeleteVendor(VendorId).ConfigureAwait(false);

                StatusClass = "alert-success";
                Message = "Vendor deleted successfully.";

                Saved = true;
            }
        }

        protected void NavigateToVendorList()
        {
            NavigationManager.NavigateTo("/vendor");
        }


    }
}
