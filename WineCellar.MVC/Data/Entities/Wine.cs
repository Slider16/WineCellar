using System.Collections.Generic;

namespace WineCellar.Net.MVC.Data.Entities
{
    public class Wine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vineyard { get; set; }
        public string Location { get; set; }
        public int Year { get; set; }
        public string Notes { get; set; }
        public List<Link> Links { get; set; }
            = new List<Link>();
        public List<WinePurchase> WinePurchases { get; set; }
            = new List<WinePurchase>();
    }
}