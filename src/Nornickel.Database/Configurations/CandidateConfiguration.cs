using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nornickel.Database.DataModels;

namespace Nornickel.Database.Configurations;

internal class CandidateConfiguration : IEntityTypeConfiguration<CandidateDataModel>
{
    public void Configure(EntityTypeBuilder<CandidateDataModel> builder)
    {
        builder.Property(c => c.Status).HasConversion<string>();
    }
}
