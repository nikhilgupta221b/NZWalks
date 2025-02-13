using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repository
{
    public class SQLRegionRepositoryImpl : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepositoryImpl(NZWalksDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            var regions = await dbContext.Region.ToListAsync();
            return regions;
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            // This will take only primary key
            // var region = await dbContext.Region.Find(id);

            // This can take any column name
            var region = await dbContext.Region.FirstOrDefaultAsync(x => x.Id == id);
            
            if (region == null)
            {
                return null;
            }

            return region;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Region.AddAsync(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, UpdateRegionRequestDto regionRequestDto)
        {
            var foundRegion = await dbContext.Region.FirstOrDefaultAsync(x => x.Id == id);

            if (foundRegion == null) 
            {
                return null;
            }
            
            // Convert DTO to Domain Model
            foundRegion.Name = regionRequestDto.Name;
            foundRegion.Code = regionRequestDto.Code;
            foundRegion.RegionImageUrl = regionRequestDto.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            return foundRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var region = await dbContext.Region.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return null;
            }

            dbContext.Region.Remove(region);
            await dbContext.SaveChangesAsync();

            return region;
        }
    }
}
