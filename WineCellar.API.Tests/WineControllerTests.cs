using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.API.AutoMapperProfiles;
using WineCellar.API.Controllers;
using WineCellar.API.Entities;
using WineCellar.API.Repositories;
using WineCellar.API.Models;
using Xunit;
using WineCellar.API.Filters;

namespace WineCellar.API.Tests
{
    public class WineControllerTests
    {
        readonly IMapper _mapper;
        private WineController _controller;

        public WineControllerTests()
        {
            var mapperConfig = new MapperConfiguration(opts =>
            {
                opts.AddProfile<WineProfile>();
                opts.AddProfile<WinePurchaseProfile>();
            });

            _mapper = mapperConfig.CreateMapper();            
        }

        [Fact]
        public async Task GetWines_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IWineRepository wineService = GetWineServiceStub();
            IWinePurchaseRepository winePurchaseService = GetWinePurchaseServiceStub();
            var response = Substitute.For<HttpResponse>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Items.Add("response", response);

            var factory = new LoggerFactory();
            var logger = factory.CreateLogger<WineController>();

            _controller = new WineController(logger, wineService, winePurchaseService, _mapper);            
            _controller.ControllerContext.HttpContext = httpContext;
            _controller.Url = Substitute.For<IUrlHelper>();

            // Act
            var actionResult = await _controller.GetWinesAsync();

            // Assert
            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetWines_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            var wineList = new List<Wine>()
            {
                new Wine() { Id = "5da3ab6639977d41082450c2", Name = "Mind Blowing Merlot", Notes = "Just goes great with everything.", Year = 2008 },
                new Wine() { Id = "5da3ad0ef351f857309400ff", Name = "Marvelous Malbec", Notes = "A wine for a quiet evening.", Year = 2011 },
                new Wine() { Id = "5da3b1d4f351f85730940101", Name = "Shining Shiraz", Notes = "This will get the party going.", Year = 2017 }
            };

            IWineRepository wineService = GetWineServiceStub(wineList);
            IWinePurchaseRepository winePurchaseService = GetWinePurchaseServiceStub();


            var response = Substitute.For<HttpResponse>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Items.Add("response", response);

            var factory = new LoggerFactory();
            var logger = factory.CreateLogger<WineController>();

            _controller = new WineController(logger, wineService, winePurchaseService, _mapper);
            _controller.ControllerContext.HttpContext = httpContext;
            _controller.Url = Substitute.For<IUrlHelper>();

            // Act
            var actionResult = await _controller.GetWinesAsync(); ;

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult.Result);
            var wines = Assert.IsType<List<WineDto>>(result.Value);
            Assert.Equal(3, wines.Count);           

        }

        [Fact]
        public async Task GetWines_WhenCalled_ReturnsWinesByVineyardStartsWith()
        {
            // Arrange
            var wineList = new List<Wine>()
            {
                new Wine() { Id = "5da3ab6639977d41082450c2", Name = "Mind Blowing Merlot", Bin = 40, Notes = "Just goes great with everything.", Year = 2008 },
                new Wine() { Id = "5da3ad0ef351f857309400ff", Name = "Marvelous Malbec", Notes = "A wine for a quiet evening.", Year = 2011 },
                new Wine() { Id = "5da3b1d4f351f85730940101", Name = "Shining Shiraz", Notes = "This will get the party going.", Year = 2017 }
            };

            IWineRepository wineService = GetWineServiceStub(wineList);
            IWinePurchaseRepository winePurchaseService = GetWinePurchaseServiceStub();

            var response = Substitute.For<HttpResponse>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Items.Add("response", response);

            var factory = new LoggerFactory();
            var logger = factory.CreateLogger<WineController>();

            _controller = new WineController(logger, wineService, winePurchaseService, _mapper);
            _controller.ControllerContext.HttpContext = httpContext;
            _controller.Url = Substitute.For<IUrlHelper>();

            // Act
            var actionResult = await _controller.GetWinesAsync(0, 0, "Aussie").ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult.Result);
            var wines = Assert.IsType<List<WineDto>>(result.Value);
            Assert.Equal(2, wines.Count);
            Assert.True(wines.TrueForAll(w => w.Vineyard.Contains("Aussie")));
        }

        [Fact]
        public async Task GetWines_WhenCalled_ReturnsWinesByYear()
        {
            // Arrange
            var wineList = new List<Wine>()
            {
                new Wine() { Id = "5da3ab6639977d41082450c2", Name = "Good Year Wine", Notes = "Just goes great with everything.", Year = 2011 },
                new Wine() { Id = "5da3ad0ef351f857309400ff", Name = "Marvelous Malbec", Notes = "A wine for a quiet evening.", Year = 2011 },
                new Wine() { Id = "5da3b1d4f351f85730940101", Name = "Shining Shiraz", Notes = "This will get the party going.", Year = 2017 }
            };

            IWineRepository wineService = GetWineServiceStub(wineList);
            IWinePurchaseRepository winePurchaseService = GetWinePurchaseServiceStub();

            var response = Substitute.For<HttpResponse>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Items.Add("response", response);

            var factory = new LoggerFactory();
            var logger = factory.CreateLogger<WineController>();

            _controller = new WineController(logger, wineService, winePurchaseService, _mapper);
            _controller.ControllerContext.HttpContext = httpContext;
            _controller.Url = Substitute.For<IUrlHelper>();

            // Act
            var actionResult = await _controller.GetWinesAsync(2011).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult.Result);
            var wines = Assert.IsType<List<WineDto>>(result.Value);
            Assert.Equal(2, wines.Count);
            Assert.True(wines.TrueForAll(w => w.Year == 2011));
        }


        [Fact]
        public async Task GetWines_WhenCalled_ReturnsWinesByYearAndBin()
        {
            // Arrange
            var wineList = new List<Wine>()
            {
                new Wine() { Id = "5da3ab6639977d41082450c2", Name = "Good Year Wine", Notes = "Just goes great with everything.", Bin = 40, Year = 2011 },
                new Wine() { Id = "5da3ad0ef351f857309400ff", Name = "Marvelous Malbec", Notes = "A wine for a quiet evening.",  Year = 2011 },
                new Wine() { Id = "5da3b1d4f351f85730940101", Name = "Shining Shiraz", Notes = "This will get the party going.", Year = 2017 }
            };

            IWineRepository wineService = GetWineServiceStub(wineList);
            IWinePurchaseRepository winePurchaseService = GetWinePurchaseServiceStub();

            var response = Substitute.For<HttpResponse>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Items.Add("response", response);

            var factory = new LoggerFactory();
            var logger = factory.CreateLogger<WineController>();

            _controller = new WineController(logger, wineService, winePurchaseService, _mapper);
            _controller.ControllerContext.HttpContext = httpContext;
            _controller.Url = Substitute.For<IUrlHelper>();

            // Act
            var actionResult = await _controller.GetWinesAsync(2011, 40).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult.Result);
            var wines = Assert.IsType<List<WineDto>>(result.Value);
            Assert.Single(wines);
            Assert.True(wines.TrueForAll(w => w.Year == 2011 && w.Bin == 40));
        }


        [Fact]
        public async Task GetWine_WhenCalled_ReturnsASingleItemBasedOnId()
        {
            // Arrange
            var wineList = new List<Wine>()
            {
                new Wine() { Id = "5da3ab6639977d41082450c2", Name = "Mind Blowing Merlot", Notes = "Just goes great with everything.", Year = 2008 },
                new Wine() { Id = "5da3ad0ef351f857309400ff", Name = "Marvelous Malbec", Notes = "A wine for a quiet evening.", Year = 2011 },
                new Wine() { Id = "5da3b1d4f351f85730940101", Name = "Shining Shiraz", Notes = "This will get the party going.", Year = 2017 }
            };

            IWineRepository wineService = GetWineServiceStub(wineList);
            IWinePurchaseRepository winePurchaseService = GetWinePurchaseServiceStub();

            var response = Substitute.For<HttpResponse>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Items.Add("response", response);

            var factory = new LoggerFactory();
            var logger = factory.CreateLogger<WineController>();

            _controller = new WineController(logger, wineService, winePurchaseService, _mapper);
            _controller.ControllerContext.HttpContext = httpContext;
            _controller.Url = Substitute.For<IUrlHelper>();

            // Act
            var actionResult = await _controller.GetWineAsync("5da3b1d4f351f85730940101");

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult.Result);
            var wine = Assert.IsType<WineDto>(result.Value);
            Assert.Equal("Shining Shiraz", wine.Name);

        }


        [Fact]
        public async Task CreateWineAsync_WhenCalled_CreatesAWineDtoToReturnToClient()
        {
            // Arrange
            IWineRepository wineService = GetWineServiceStub();
            IWinePurchaseRepository winePurchaseService = GetWinePurchaseServiceStub();

            var response = Substitute.For<HttpResponse>();
            
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Items.Add("response", response);

            var factory = new LoggerFactory();
            var logger = factory.CreateLogger<WineController>();

            _controller = new WineController(logger, wineService, winePurchaseService, _mapper);
            _controller.ControllerContext.HttpContext = httpContext;
            _controller.Url = Substitute.For<IUrlHelper>();

            // Act
            var wineForCreationDto = new WineForCreationDto()
            {
                Name = "Red Wine",
                Vineyard = "My Vineyard"
            };
            var actionResult = await _controller.CreateWineAsync(wineForCreationDto).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
            var wineDtoReturnedToClient = Assert.IsType<WineDto>(result.Value);
            Assert.Equal("Red Wine", wineDtoReturnedToClient.Name);
        }


        private IWinePurchaseRepository GetWinePurchaseServiceStub(List<WinePurchase> winePurchaseList = null)
        {
            return winePurchaseList != null ? new WinePurchaseServiceFake(winePurchaseList) : new WinePurchaseServiceFake();
        }

        private IWineRepository GetWineServiceStub(List<Wine> wineList = null)
        {
            return wineList != null ? new WineServiceFake(wineList) : new WineServiceFake();
        }
    }
}
