using ApiCafe.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCafe.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    public DbSet<CafeEntity> Cafes => Set<CafeEntity>();
}
