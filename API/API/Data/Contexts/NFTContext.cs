using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Contexts
{
    public class NFTContext : DbContext
    {
        public DbSet<NFT> NFTs { get; set; }

        public NFTContext(DbContextOptions<NFTContext> options)
            : base(options)
        {
        }
    }
}
