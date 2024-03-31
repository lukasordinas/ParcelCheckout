using Microsoft.EntityFrameworkCore;

namespace ParcelCheckout.Api.Data.Configuration;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions options)
        : base(options)
    {
    }

    public virtual DbSet<Service> Services => Set<Service>();
}
