using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data;

public class NZWalksAuthDbContext : IdentityDbContext
{
    public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "3F2504E0-4F89-41D3-9A0C-0305E82C3301";
        var writerRoleId = "A6F2C90B-0D5A-4F77-925D-1D81C03D8F59";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = readerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = readerRoleId,
                NormalizedName = readerRoleId.ToUpper()
            },
            new IdentityRole
            {
                Id = writerRoleId,
                ConcurrencyStamp = writerRoleId,
                Name = writerRoleId,
                NormalizedName = writerRoleId.ToUpper()
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}