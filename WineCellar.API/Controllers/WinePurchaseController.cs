using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WineCellar.API.Entities;
using WineCellar.API.Repositories;
using WineCellar.API.Models;

namespace WineCellar.API.Controllers
{
    /// <summary>
    /// The MVC based controller for WinePurchases without view support
    /// </summary>
    [Produces("application/json", "application/xml")]
    [Route("api/[controller]")]
    [ApiController]
    public class WinePurchaseController : ControllerBase
    {
        private readonly ILogger<WinePurchaseController> _logger;
        private readonly IWinePurchaseRepository _service;
        private readonly IWineRepository _wineService;
        private readonly IMapper _mapper;

        /// <summary>
        /// The main WinePurchaseController constructor with parameters necessary for dependency injection
        /// </summary>
        /// <param name="logger">The ILogger type of WinePurchaseController for dependency injection</param>
        /// <param name="service">The IWinePurchaseService for dependency injection</param>
        /// <param name="wineService">The IWineService for dependency injection</param>
        /// <param name="mapper">Depenpency injection for AutoMapper</param>
        public WinePurchaseController(ILogger<WinePurchaseController> logger, IWinePurchaseRepository service, IWineRepository wineService, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _wineService = wineService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a paged list, size a page count based on query string, of all wine purchases for the specified wine
        /// </summary>
        /// <param name="wineId">The Id of the Wine for which to retrieve purchases</param>
        /// <returns>An ActionResult of type IEnumerable of WinePurchaseDto</returns>
        [HttpGet(Name = "GetWinePurchases")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<WinePurchaseDto>>> GetWinePurchases(string wineId)
        {
            if (!await _wineService.WineExists(wineId).ConfigureAwait(false))
            {
                return NotFound();
            }

            var winePurchasesFromService = await _service.GetWinePurchasesAsync(wineId).ConfigureAwait(false);

            var winePurchasesDto = _mapper.Map<IEnumerable<WinePurchaseDto>>(winePurchasesFromService);

            return Ok(winePurchasesDto);
        }

        /// <summary>
        /// Get a wine purchase by wineId and purchaseId
        /// </summary>
        /// <param name="wineId">The Id of the Wine for which to retrieve a purchase</param>
        /// <param name="winePurchaseId"></param>
        /// <returns>An ActionResult of type WinePurchaseDto</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{winePurchaseId}", Name = "GetWinePurchaseAsync")]
        public async Task<ActionResult<WinePurchaseDto>> GetWinePurchaseAsync(string wineId, string winePurchaseId)
        {
            if (!await _wineService.WineExists(wineId).ConfigureAwait(false))
            {
                return NotFound();
            }

            var winePurchaseFromService = await _service.GetWinePurchaseAsync(wineId, winePurchaseId).ConfigureAwait(false);

            var winePurchaseDto = _mapper.Map<WinePurchaseDto>(winePurchaseFromService);

            return Ok(winePurchaseDto);
        }

        /// <summary>
        /// Create a new wine purchase
        /// </summary>
        /// <param name="wineId">The Id of the Wine for which to create a purchase</param>
        /// <param name="winePurchaseForCreationDto">A data transfer object respresentation of the WinePurchase to create</param>
        /// <returns>An ActionResult of type WinePurchaseDto</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<WinePurchaseDto>> CreateWinePurchaseAsync(string wineId, WinePurchaseForCreationDto winePurchaseForCreationDto)
        {
            var winePurchaseEntity = _mapper.Map<WinePurchase>(winePurchaseForCreationDto);

            var winePurchaseAdded = await _service.CreateWinePurchaseAsync(winePurchaseEntity).ConfigureAwait(false);

            // See notes in WineController.CreateWineAsync() for explanation of this mapping
            var winePurchaseDtoToReturn = _mapper.Map<WinePurchaseDto>(winePurchaseAdded);

            return CreatedAtRoute(nameof(GetWinePurchaseAsync),
                new { wineId, winePurchaseId = winePurchaseDtoToReturn.Id }, winePurchaseDtoToReturn);
        }

        /// <summary>
        /// Update a wine purchase.  Creates new wine purchase if wine purchase not found
        /// </summary>
        /// <param name="wineId">The id of the Wine for which to update a purchase</param>
        /// <param name="winePurchaseId">The id of WinePurchase to update</param>
        /// <param name="winePurchaseForUpdateDto">A data transfer object representation of the WinePurchase to update</param>
        [HttpPut("{winePurchaseId}", Name = "UpdateWinePurchaseAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateWinePurchaseAsync(string wineId, string winePurchaseId, WinePurchaseForUpdateDto winePurchaseForUpdateDto)
        {
            if (!await _wineService.WineExists(wineId).ConfigureAwait(false))
            {
                return NotFound();
            }

            var winePurchaseEntityToUpdate = await _service.GetWinePurchaseAsync(wineId, winePurchaseId).ConfigureAwait(false);

            if (winePurchaseEntityToUpdate == null)
            {
                // Do an UPSERT
                var winePurchaseToAdd = _mapper.Map<WinePurchase>(winePurchaseForUpdateDto);

                winePurchaseToAdd.Id = winePurchaseId;

                var winePurchaseAdded = await _service.CreateWinePurchaseAsync(winePurchaseToAdd).ConfigureAwait(false);

                var winePurchaseDtoToReturn = _mapper.Map<WinePurchaseDto>(winePurchaseToAdd);

                return CreatedAtRoute(nameof(GetWinePurchaseAsync),
                    new { wineId, winePurchaseId = winePurchaseDtoToReturn.Id }, winePurchaseDtoToReturn);
            }

            // Map the changes from our winePurchaseForUpdateDto to the entity in winePurchaseEntityToUpdate
            _mapper.Map(winePurchaseForUpdateDto, winePurchaseEntityToUpdate);

            // Update the database
            await _service.UpdateWinePurchaseAsync(winePurchaseId, winePurchaseEntityToUpdate).ConfigureAwait(false);

            return NoContent();
        }

        /// <summary>
        /// Delete a WinePurchase by id
        /// </summary>
        /// <param name="wineId">The id of the Wine for which to delete a purchase</param>
        /// <param name="winePurchaseId">The id of the WinePurchase to delete</param>
        [HttpDelete("{winePurchaseId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteWinePurchaseAsync(string wineId, string winePurchaseId)
        {
            if (!await _wineService.WineExists(wineId).ConfigureAwait(false))
            {
                return NotFound();
            }

            var winePurchaseFromService = await _service.GetWinePurchaseAsync(wineId, winePurchaseId).ConfigureAwait(false);
            if (winePurchaseFromService == null)
            {
                return NoContent();
            }

            await _service.DeleteWinePurchaseAsync(winePurchaseId).ConfigureAwait(false);

            return NoContent();
        }
    }
}
