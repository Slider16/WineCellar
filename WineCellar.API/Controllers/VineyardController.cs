using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using WineCellar.API.Entities;
using WineCellar.API.Models;
using WineCellar.API.Repositories;

namespace WineCellar.API.Controllers
{
    /// <summary>
    /// The MVC based controller for Vineyards without view support
    /// </summary>
    [Produces("application/json", "application/xml")]
    [Route("api/[Controller]")]
    [ApiController]
    public class VineyardController : Controller
    {
        private readonly ILogger<VineyardController> _logger;
        private readonly IVineyardRepository _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// The main VineyardController constructor with parameters necessary for dependency injection
        /// </summary>
        /// <param name="logger">The ILogger type of VineyardController for dependency injection</param>
        /// <param name="service">The IVineyardRepository for dependency injection</param>
        /// <param name="mapper">Dependency injection for AutoMapper</param>
        public VineyardController(ILogger<VineyardController> logger, IVineyardRepository service, IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            _service = service ??
                throw new ArgumentNullException(nameof(service));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get a paged list of all vineyards in the database.  List size and page count based on query string
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of VineyardDto</returns>
        [HttpGet(Name = "GetVineyardsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VineyardDto>>> GetVineyardsAsync()
        {
            var vineyardsFromService = await _service.GetVineyardsAsync().ConfigureAwait(false);

            var vineyardDtos = _mapper.Map<IEnumerable<VineyardDto>>(vineyardsFromService);

            //Add a "links" collection to each vineyard object returned.
            foreach (var vineyardDto in vineyardDtos)
            {
                vineyardDto.Links = CreateLinksForVineyard(vineyardDto);
            }

            return Ok(vineyardDtos);
        }

        /// <summary>
        /// Get a vineyard by id
        /// </summary>
        /// <param name="id">The id of the Vineyard to get</param>
        /// <returns>An ActionResult of type VineyardDto</returns>
        [HttpGet("{id}", Name = "GetVineyardAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VineyardDto>> GetVineyardAsync(string id)
        {
            var vineyardFromService = await _service.GetVineyardAsync(id).ConfigureAwait(false);
            if (vineyardFromService == null)
            {
                return NotFound();
            }

            var vineyardDto = _mapper.Map<VineyardDto>(vineyardFromService);

            // Add a "links collection" to the vineyardDto object returned.
            vineyardDto.Links = CreateLinksForVineyard(vineyardDto);

            return Ok(vineyardDto);
        }

        /// <summary>
        /// Create a new Vineyard
        /// </summary>
        /// <param name="vineyardForCreationDto">A data transfer object representation of the Vineyard to create</param>
        /// <returns>An ActionResult of type VineyardDto</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<VineyardDto>> CreateVineyardAsync(VineyardForCreationDto vineyardForCreationDto)
        {
            // TODO: Setup "REQUIRED" attributes on your entities to replace this code.
            // Maybe implement some custom "Validation Attributes" just for practice.
            //if (string.IsNullOrEmpty(vineyardForCreationDto.Name))
            //{
            //    var msg = "No Vineyard name found in request.";
            //    _logger.LogError(msg);
            //    return UnprocessableEntity(new { status = "Error:", message = msg });

            //}

            var vineyardEntity = _mapper.Map<Vineyard>(vineyardForCreationDto);

            // Get the new "vineyard added" that contains the vineyard Id from the datastore
            var vineyardAdded = await _service.CreateVineyardAsync(vineyardEntity).ConfigureAwait(false);

            // Map the new vineyard to a "presentation vineyard" or DTO vineyard
            // Yeah, yeah, I could just update the Id on the vineyardDto that was passed in and return that
            // but, I'd rather be consistent with the vineyard that is presented to the client, always a VineyardDto.  
            // And besides, we do all the mapping in one place, in the MAPPER.
            var vineyardDtoToReturn = _mapper.Map<VineyardDto>(vineyardAdded);

            vineyardDtoToReturn.Links = CreateLinksForVineyard(vineyardDtoToReturn);

            // Return the DTO vineyard
            return CreatedAtRoute(nameof(GetVineyardAsync), new { id = vineyardDtoToReturn.Id }, vineyardDtoToReturn);
        }


        /// <summary>
        /// Update a Vineyard.  Creates a new Vineyard entity if vineyard not found
        /// </summary>
        /// <param name="id">The id of the Vineyard to update</param>
        /// <param name="vineyardForUpdateDto">A data transfer object representation of the Vineyard to update or create if not found</param>
        /// <returns>An ActionResult of NoContent</returns>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateVineyardAsync(string id, VineyardForUpdateDto vineyardForUpdateDto)
        {
            var vineyardEntityToUpdate = await _service.GetVineyardAsync(id).ConfigureAwait(false);

            if (vineyardEntityToUpdate == null)
            {                
                var vineyardToAdd = _mapper.Map<Vineyard>(vineyardForUpdateDto);

                // This is what makes this PUT idempotent
                vineyardToAdd.Id = id;

                var vineyardAdded = await _service.CreateVineyardAsync(vineyardToAdd).ConfigureAwait(false);

                var vineyardDtoToReturn = _mapper.Map<VineyardDto>(vineyardAdded);

                return CreatedAtRoute(nameof(GetVineyardAsync), new { vineyardDtoToReturn.Id }, vineyardDtoToReturn);
            }

            // Map the changes from our vineyardForUpdateDto to the entity vineyardToUpdate
            _mapper.Map(vineyardForUpdateDto, vineyardEntityToUpdate);

            // Update the database
            await _service.UpdateVineyardAsync(id, vineyardEntityToUpdate).ConfigureAwait(false);

            return NoContent();
        }

        /// <summary>
        /// Delete a Vineyard by id
        /// </summary>
        /// <param name="id">The id of the Vineyard to delete</param>
        /// <returns>An ActionResult of type NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteVineyardAsync(string id)
        {
            var vineyardFromService = await _service.GetVineyardAsync(id).ConfigureAwait(false);
            if (vineyardFromService == null)
            {
                return NotFound();
            }

            await _service.DeleteVineyardAsync(id).ConfigureAwait(false);

            return NoContent();
        }

        private List<LinkDto> CreateLinksForVineyard(VineyardDto vineyardDto)
        {
            var links = new List<LinkDto>
            {
                // Add the "filter_by_id" link
                new LinkDto(Url.Link(nameof(GetVineyardAsync), new { id = vineyardDto.Id }), "filter_by_id", "GET")
            };

            return links;
        }

    }
}
