using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class SQLWalkRepositoryImpl : IWalkRepository 
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepositoryImpl(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            // This will not pass Difficulty or Region objects inside walk object
            // List<Walk> walks = await dbContext.Walks.ToListAsync();
            
            // Uses Navigation Properties
            List<Walk> walks = await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

            return walks;
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);

            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkFound = await dbContext.Walks.FirstOrDefaultAsync();

            if (walk == null)
            {
                return null;
            }

            walkFound.Name = walk.Name;
            walkFound.Description = walk.Description;
            walkFound.LengthInKm = walk.LengthInKm;
            walkFound.WalkImageUrl = walk.WalkImageUrl;
            walkFound.RegionId = walk.RegionId;
            walkFound.DifficultyId = walk.DifficultyId;

            await dbContext.SaveChangesAsync();

            return walkFound;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var foundWalkDomainModel = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (foundWalkDomainModel == null)
            {
                return null;
            }

            dbContext.Remove(foundWalkDomainModel);
            await dbContext.SaveChangesAsync();

            return foundWalkDomainModel;
        }
    }
}
