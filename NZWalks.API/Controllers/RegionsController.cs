using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public RegionsController(IRegionRepository regionRepository)
        {
            this.regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
            // Get Models from DB
            List<Region> regionDomains = await regionRepository.GetAllAsync();
            
            // Convert Models to DTOs
            List<RegionDto> regionDtos = new List<RegionDto>();

            foreach (var regionDomain in regionDomains)
            {
                regionDtos.Add(new RegionDto
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl,
                });
            }

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
            RegionDto regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            // Return to client
            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto requestDto)
        {
            // Convert DTO to Region Domain
            Region region = new Region
            {
                Name = requestDto.Name,
                Code = requestDto.Code,
                RegionImageUrl = requestDto.RegionImageUrl,
            };

            // Save Region to DB
            await regionRepository.CreateAsync(region);

            // Convert back to Response DTO
            RegionDto regionDto = new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id}, regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Check if it exists in db
            var regionDomainModel = await regionRepository.UpdateAsync(id, updateRegionRequestDto);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain model to Response DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

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
            RegionDto regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
