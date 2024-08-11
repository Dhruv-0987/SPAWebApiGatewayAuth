using Microsoft.EntityFrameworkCore;
using ResultsPatternMinimalApis.Models;

namespace ResultsPatternMinimalApis.Database;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    
    public DbSet<User> Users { get; set; } = default!;
}