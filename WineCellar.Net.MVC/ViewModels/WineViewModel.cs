using System.Collections.Generic;

namespace WineCellar.Net.MVC.ViewModels
{
    public class WineViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vineyard { get; set; }
        public string Location { get; set; }
        public int Year { get; set; }
        public string Notes { get; set; }
        public List<LinkViewModel> Links { get; set; }
            = new List<LinkViewModel>();
        public List<WinePurchaseViewModel> WinePurchases { get; set; }
            = new List<WinePurchaseViewModel>();
    }
}
