using System;
using System.Collections.Generic;
using System.Text;

namespace WineCellar.Blazor.Shared.Models
{
    public class Wine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vineyard { get; set; }
        public string Location { get; set; }
        public int Year { get; set; }
        public string Notes { get; set; }
    }
}
