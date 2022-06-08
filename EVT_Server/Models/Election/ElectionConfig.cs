using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVT_Server.Models.Election{

public class ElectionConfig : IEntityTypeConfiguration<ElectionInfo>
{
    public void Configure(EntityTypeBuilder<ElectionInfo> builder)
    {
        throw new NotImplementedException();
    }
}
}