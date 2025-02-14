using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
            // Get Models from DB
            List<Region> regionDomains = await regionRepository.GetAllAsync();

            // Convert Domain models to Dtos
            List<RegionDto> regionDtos = mapper.Map<List<RegionDto>>(regionDomains);

            // Return to client
            return Ok(regionDtos);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {   
            // Get Region Domain Model by id
            var regionDomain = await regionRepository.GetByIdAsync(id);

            // If null return NotFound()
            if (regionDomain == null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            RegionDto regionDto = mapper.Map<RegionDto>(regionDomain);

            // Return to client
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto requestDto)
        {
            // Convert DTO to Region Domain
            Region region = mapper.Map<Region>(requestDto);

            // Save Region to DB
            await regionRepository.CreateAsync(region);

            // Convert back to Response DTO
            RegionDto regionDto = mapper.Map<RegionDto>(region);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Check if it exists in db
            var regionDomainModel = await regionRepository.UpdateAsync(id, updateRegionRequestDto);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain model to Response DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            // Return DTO to client
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Get Region from DB
            var region = await regionRepository.DeleteAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            // Convert Domain Model to Response DTO
            RegionDto regionDto = mapper.Map<RegionDto>(region);

            return Ok(regionDto);
        }
    }
}
