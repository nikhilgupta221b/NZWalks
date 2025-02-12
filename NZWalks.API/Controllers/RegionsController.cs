using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllRegions()
        {
            // Get regions from db
            var regionDomains = dbContext.Region.ToList();
            
            // Map Domain Model to DTO
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
        [Route("{id}")]
        public IActionResult GetRegionById(Guid id)
        {   
            // This will take only primary key
            // Region region = dbContext.Region.Find(id);
            
            // This can take Other column names as well.
            var regionDomain = dbContext.Region.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO
            RegionDto regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            // return to client
            return Ok(regionDto);
        }
    }
}
