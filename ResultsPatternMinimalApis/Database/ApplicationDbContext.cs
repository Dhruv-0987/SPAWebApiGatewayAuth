using Microsoft.EntityFrameworkCore;
using ResultsPatternMinimalApis.Models;

namespace ResultsPatternMinimalApis.Database;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    
    public DbSet<User> Users { get; init; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}