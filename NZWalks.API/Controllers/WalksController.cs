using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkReqeustDto addWalkReqeustDto)
        {
            // Map Request Dto to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkReqeustDto);

            await walkRepository.CreateAsync(walkDomainModel);

            WalkDto walkDto = mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walkDomainModels = await walkRepository.GetAllAsync();

            var walkDtos = mapper.Map<List<WalkDto>>(walkDomainModels);

            return Ok(walkDtos);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDto = mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            var updatedWalkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (updatedWalkDomainModel == null)
            {
                return NotFound();
            }

            var updatedWalkDto = mapper.Map<WalkDto>(updatedWalkDomainModel);

            return Ok(updatedWalkDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var foundWalk = await walkRepository.DeleteAsync(id);

            if (foundWalk == null)
            {
                return NotFound();
            }

            var deletedWalkDto = mapper.Map<WalkDto>(foundWalk);

            return Ok(deletedWalkDto);
        }
    }
}
