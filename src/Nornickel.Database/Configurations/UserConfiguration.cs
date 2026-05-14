using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nornickel.Database.DataModels;

namespace Nornickel.Database.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<UserDataModel>
{
    public void Configure(EntityTypeBuilder<UserDataModel> builder)
    {
        builder.Property(u => u.Role).HasConversion<string>();
        builder.HasIndex(u => u.Username).IsUnique();
    }
}
