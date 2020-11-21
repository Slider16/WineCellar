using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using WineCellar.Net.API.Entities;
using WineCellar.Net.API.Repositories;
using WineCellar.Net.API.Models;

namespace WineCellar.Net.API.Controllers
{
    /// <summary>
    /// The MVC based controller for Vendors without View support
    /// </summary>
    [Produces("application/json", "application/xml")]
    [Route("api/vendors")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly ILogger<VendorController> _logger;
        private readonly IVendorRepository _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// The main VendorController constructor with parameters necessary for dependency injection
        /// </summary>
        /// <param name="logger">The ILogger type of VendorController for dependency injection</param>
        /// <param name="service">The IVendorService for dependency injection</param>
        /// <param name="mapper">Dependency injection for AutoMapper</param>
        public VendorController(ILogger<VendorController> logger, IVendorRepository service, IMapper mapper)
        {
            _logger = logger ??
               throw new ArgumentNullException(nameof(logger));

            _service = service ??
                throw new ArgumentNullException(nameof(service));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get a paged list, size a page count based on query string, of all vendors in the database
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of VendorDto</returns>
        [HttpGet(Name = "GetVendorsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VendorDto>>> GetVendorsAsync()
        {
            var vendorsFromService = await _service.GetVendorsAsync().ConfigureAwait(false);

            var vendorDtos = _mapper.Map<IEnumerable<VendorDto>>(vendorsFromService);

            //Add a "links" collection to each launchpad object returned.
            foreach (var vendorDto in vendorDtos)
            {
                vendorDto.Links = CreateLinksForVendor(vendorDto);
            }

            return Ok(vendorDtos);
        }

        /// <summary>
        /// Get a vendor by id
        /// </summary>
        /// <param name="id">The id of the Vendor to get</param>
        /// <returns>An ActionResult of type VendorDto</returns>
        [HttpGet("{id}", Name = "GetVendorAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VendorDto>> GetVendorAsync(string id)
        {
            var vendorFromService = await _service.GetVendorAsync(id).ConfigureAwait(false);
            if (vendorFromService == null)
            {
                return NotFound();
            }

            var vendorDto = _mapper.Map<VendorDto>(vendorFromService);

            // Add a "links collection" to the vendorDto object returned.
            vendorDto.Links = CreateLinksForVendor(vendorDto);

            return Ok(vendorDto);
        }

        /// <summary>
        /// Create a new Vendor
        /// </summary>
        /// <param name="vendorForCreationDto">A data transfer object representation of the Vendor to create</param>
        /// <returns>An ActionResult of type VendorDto</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<VendorDto>> CreateVendorAsync(VendorForCreationDto vendorForCreationDto)
        {
            // TODO: Setup "REQUIRED" attributes on your entities to replace this code.
            // Maybe implement some custom "Validation Attributes" just for practice.
            //if (string.IsNullOrEmpty(vendorForCreationDto.Name))
            //{
            //    var msg = "No Vendor name found in request.";
            //    _logger.LogError(msg);
            //    return UnprocessableEntity(new { status = "Error:", message = msg });

            //}

            var vendorEntity = _mapper.Map<Vendor>(vendorForCreationDto);

            // Get the new "vendor added" that contains the vendor Id from the datastore
            var vendorAdded = await _service.CreateVendorAsync(vendorEntity).ConfigureAwait(false);

            // Map the new vendor to a "presentation vendor" or DTO vendor
            // Yeah, yeah, I could just update the Id on the vendorDto that was passed in and return that
            // but, I'd rather be consistent with the vendor that is presented to the client, always a VendorDto.  
            // And besides, we do all the mapping in one place, in the MAPPER.
            var vendorDtoToReturn =  _mapper.Map<VendorDto>(vendorAdded);

            vendorDtoToReturn.Links = CreateLinksForVendor(vendorDtoToReturn);

            // Return the DTO vendor
            return CreatedAtRoute(nameof(GetVendorAsync), new { id = vendorDtoToReturn.Id }, vendorDtoToReturn);
        }

        /// <summary>
        /// Update a Vendor.  Creates a new Vendor entity if vendor not found
        /// </summary>
        /// <param name="id">The id of the Vendor to update</param>
        /// <param name="vendorForUpdateDto">A data transfer object representation of the Vendor to update or create if not found</param>
        /// <returns>An ActionResult of type NoContent</returns>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateVendorAsync(string id, VendorForUpdateDto vendorForUpdateDto)
        {
            var vendorEntityToUpdate = await _service.GetVendorAsync(id).ConfigureAwait(false);

            if(vendorEntityToUpdate == null)
            {
                // Do an UPSERT
                var vendorToAdd = _mapper.Map<Vendor>(vendorForUpdateDto);

                // This is what makes this PUT idempotent
                vendorToAdd.Id = id;

                var vendorAdded = await _service.CreateVendorAsync(vendorToAdd).ConfigureAwait(false);

                var vendorDtoToReturn = _mapper.Map<VendorDto>(vendorAdded);

                return CreatedAtRoute(nameof(GetVendorAsync), new { vendorDtoToReturn.Id }, vendorDtoToReturn);
            }

            // Map the changes from our vendorDto to the entity vendorToUpdate
            _mapper.Map(vendorForUpdateDto, vendorEntityToUpdate);

            // Update the databse
            await _service.UpdateVendorAsync(id, vendorEntityToUpdate).ConfigureAwait(false);

            return NoContent();
        }

        /// <summary>
        /// Deletes a vendor by id
        /// </summary>
        /// <param name="id">The id of the Vendor to delete</param>
        /// <returns>An ActionResult of type NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteVendorAsync(string id)
        {
            var vendorFromService = _service.GetVendorAsync(id);
            if (vendorFromService == null)
            {
                return NotFound();
            }

            await _service.DeleteVendorAsync(id).ConfigureAwait(false);

            return NoContent();
        }

        private List<LinkDto> CreateLinksForVendor(VendorDto vendorDto)
        {
            var links = new List<LinkDto>
            {
                // Add the "filter_by_id" link
                new LinkDto(Url.Link(nameof(GetVendorAsync), new { id = vendorDto.Id }), "filter_by_id", "GET")
            };

            return links;
        }
    }
}
