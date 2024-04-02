using Microsoft.EntityFrameworkCore;

namespace ParcelCheckout.Data.Configuration;

public class DbContext(DbContextOptions options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public virtual DbSet<Service> Services => Set<Service>();
}
