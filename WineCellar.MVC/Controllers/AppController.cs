using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WineCellar.Net.MVC.Services;
using WineCellar.Net.MVC.ViewModels;

namespace WineCellar.Net.MVC.Controllers
{
    public class AppController : Controller
    {
        private readonly IWineService _winesService;
        private readonly IMapper _mapper;
        public AppController(IWineService winesService, IMapper mapper)
        {
            _winesService = winesService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult<IEnumerable<WineViewModel>>> WineList()
        {
            var winesFromService = await _winesService.GetWinesAsync().ConfigureAwait(false);

            var wineViewModels = _mapper.Map<IEnumerable<WineViewModel>>(winesFromService);

            return View(wineViewModels);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult WineForm()
        {
            return View();
        }
    }
}