using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WineCellar.Net.API.Entities;
using WineCellar.Net.API.Repositories;
using WineCellar.Net.API.Models;

namespace WineCellar.Net.API.Controllers
{
    /// <summary>
    /// The MVC based controller for Wines without view support
    /// </summary>
    [Produces("application/json", "application/xml")]
    [Route("api/wines")]
    [ApiController]   
    public class WineController : ControllerBase
    {
        private readonly ILogger<WineController> _logger;
        private readonly IWineRepository _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// The main WineController constructor with parameters necessary for dependency injection
        /// </summary>
        /// <param name="logger">The ILogger type of VendorController for dependency injection</param>
        /// <param name="service">The IVendorService for dependency injection</param>
        /// <param name="mapper">Dependency injection for AutoMapper</param>
        public WineController(ILogger<WineController> logger, IWineRepository service, IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            
            _service = service ??
                throw new ArgumentNullException(nameof(service));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

        }

        /// <summary>
        /// Get a paged list of all wines in the database. List size and page count based on query string
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of WineDto</returns>
        [HttpGet(Name = "GetWinesAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WineDto>>> GetWinesAsync()
        {
            var winesFromService = await _service.GetWinesAsync().ConfigureAwait(false);

            var wineDtos = _mapper.Map<IEnumerable<WineDto>>(winesFromService);

            //Add a "links" collection to each Wine object returned.
            foreach(var wineDto in wineDtos)
            {
                wineDto.Links = CreateLinksForWine(wineDto);
            }

            return Ok(wineDtos);
        }

        /// <summary>
        /// Get a wine by id
        /// </summary>
        /// <param name="id">The id of the Wine to get</param>
        /// <returns>An ActionResult of type WineDto</returns>
        [HttpGet("{id}", Name = "GetWineAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WineDto>> GetWineAsync(string id)
        {
            _logger.LogDebug("Getting wine from serivce.");

            var wineFromService = await _service.GetWineAsync(id).ConfigureAwait(false);
            if (wineFromService == null)
            {
                return NotFound();
            }

            _logger.LogDebug("Mapping wine from service to WineDto.");
            
            var wineDto = _mapper.Map<WineDto>(wineFromService);

            // Add a "links collection" to the wineDto object returned.
            wineDto.Links = CreateLinksForWine(wineDto);

            return Ok(wineDto);
        }

        /// <summary>
        /// Create a new wine
        /// </summary>
        /// <param name="wineForCreationDto">A data transfer object representation of the Wine to create</param>
        /// <returns>An ActionResult of type WineDto</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<WineDto>> CreateWineAsync(WineForCreationDto wineForCreationDto)
        {
            // TODO: Setup "REQUIRED" attributes on your entities to replace this code.
            // Maybe implement some custom "Validation Attributes" just for practice.
            //if (string.IsNullOrEmpty(wineForCreationDto.Name))
            //{
            //    var msg = "No Wine name found in request.";
            //    _logger.LogError(msg);
            //    return BadRequest(new { status = "Error:", message = msg });
            //}

            var wineEntity = _mapper.Map<Wine>(wineForCreationDto);

            var wineAdded = await _service.CreateWineAsync(wineEntity).ConfigureAwait(false);

            // Map the new wine to a "presentation wine" or DTO wine
            // Yeah, yeah, I could just update the Id on the wineForCreationDto that was passed 
            // in and return that.  But, we should be consistent with the wine that is 
            // presented to the client, always a WineDto.
            // And besides we  do all the mapping in one place, in the MAPPER.
            var wineDtoToReturn = _mapper.Map<WineDto>(wineAdded);

            wineDtoToReturn.Links = CreateLinksForWine(wineDtoToReturn);
            
            return CreatedAtRoute(nameof(GetWineAsync), new { id = wineDtoToReturn.Id }, wineDtoToReturn);
        }

        /// <summary>
        /// Update a Wine.  Creates a new Wine entity if wine not found
        /// </summary>
        /// <param name="id">The id of the Wine to update</param>
        /// <param name="wineForUpdateDto">A data transfer object representation of the Wine to update or create if not found</param>
        /// <returns>An ActionResult of NoContent</returns>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateWineAsync(string id, WineForUpdateDto wineForUpdateDto)
        {
            var wineEntityToUpdate = await _service.GetWineAsync(id).ConfigureAwait(false);

            if (wineEntityToUpdate == null)
            {
                // Do an UPSERT
                var wineToAdd = _mapper.Map<Wine>(wineForUpdateDto);

                // This is what makes this PUT idempotent
                wineToAdd.Id = id;

                var wineAdded = await _service.CreateWineAsync(wineToAdd).ConfigureAwait(false);

                var wineDtoToReturn = _mapper.Map<WineDto>(wineAdded);

                return CreatedAtRoute(nameof(GetWineAsync), new { wineDtoToReturn.Id }, wineDtoToReturn);                
            }

            // Map the changes from our wineForUpdateDto to the entity wineToUpdate
            _mapper.Map(wineForUpdateDto, wineEntityToUpdate);

            // Update the database
            await _service.UpdateWineAsync(id, wineEntityToUpdate).ConfigureAwait(false);

            return NoContent();
        }

        /// <summary>
        /// Delete a Wine by id
        /// </summary>
        /// <param name="id">The id of the Wine to delete</param>
        /// <returns>An ActionResult of type NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteWineAsync(string id)
        {
            var wineFromService = await _service.GetWineAsync(id).ConfigureAwait(false);
            if (wineFromService == null)
            {
                return NotFound();
            }

            await _service.DeleteWineAsync(id).ConfigureAwait(false);

            return NoContent();
        }

        private List<LinkDto> CreateLinksForWine(WineDto wineDto)
        {
            var links = new List<LinkDto>
            {
                // Add the "filter_by_id" link
                new LinkDto(Url.Link(nameof(GetWineAsync), new { id = wineDto.Id }), "filter_by_id", "GET")
            };
            
            return links;
        }
    }
}
