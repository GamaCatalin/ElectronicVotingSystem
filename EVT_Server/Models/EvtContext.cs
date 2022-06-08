using EVT_Server.Models.Election;
using Microsoft.EntityFrameworkCore;

namespace EVT_Server.Models{

public class EvtContext : DbContext
{
    public EvtContext()
    {
    }

    public EvtContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<ElectionInfo> Elections { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ElectionConfig());
    }
    
}
}