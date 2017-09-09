using Microsoft.EntityFrameworkCore;
using vega.Models;

namespace vega.Persistance
{
    public class VegaDbContext : DbContext
    {
        public DbSet<Make> Make { get; set; }  // EF will figure out that it need to create Model as well with navigation property in Make class
        
        public VegaDbContext(DbContextOptions<VegaDbContext> options) : base(options)
        {
            
        }
    }
}