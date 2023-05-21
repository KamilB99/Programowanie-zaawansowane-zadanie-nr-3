using Microsoft.EntityFrameworkCore;

namespace AdvancedProgramming_Lesson3.Models
{
    public class OsobaContext : DbContext
    {
        
        public OsobaContext(DbContextOptions<OsobaContext> options)
            : base(options)
        {
        }

        public DbSet<OsobaItem> OsobaItems { get; set; }
    }
}
