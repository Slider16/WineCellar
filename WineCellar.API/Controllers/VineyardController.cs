using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.API.Controllers
{
    public class VineyardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
