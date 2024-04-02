using Microsoft.EntityFrameworkCore;

namespace ParcelCheckout.Data.Configuration;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public virtual DbSet<Service> Services => Set<Service>();

    public DbContext()
        : base()
    {
    }
}
