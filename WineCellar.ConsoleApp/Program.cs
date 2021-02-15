using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WineCellar.ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var http = new HttpClient();
            
            var client = new SwaggerClient("http://localhost:7777", http);

            //VendorForCreationDto newVendor = new VendorForCreationDto { Name = "Another Console Vendor", City = "Lebanon", State = "TN" };
            //var result = await client.VendorAsync(newVendor).ConfigureAwait(false);

            var vendors = await client.GetVendorsAsync().ConfigureAwait(false);

            foreach(var vendor in vendors)
            {
                Console.WriteLine(vendor.Name);
            }

            var wine = (await client.GetWinesAsync(null, null, string.Empty).ConfigureAwait(false)).FirstOrDefault();

            var winePurchases = await client.GetWinePurchasesAsync(wine?.Id).ConfigureAwait(false);

            foreach(var purchase in winePurchases)
            {
                Console.WriteLine($"WineId: {purchase.WineId} PurchaseId: {purchase.Id}  Purchase Date: {purchase.PurchaseDate} ");
            }
            
        }
    }
}
