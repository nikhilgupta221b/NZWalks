using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) {
            
        }

        DbSet<Walk> Walks { get; set; }
        DbSet<Difficulty> Difficulty { get; set; }
        DbSet<Region> Region { get; set; }
    }
}
