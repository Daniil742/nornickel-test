using Microsoft.EntityFrameworkCore;
using Nornickel.Database.Configurations;
using Nornickel.Database.Enums;
using Npgsql;

namespace Nornickel.Database.Contexts;

public class HrSystemDbContext : DbContext
{
    public HrSystemDbContext(DbContextOptions<HrSystemDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresEnum<UserRole>(name: "UserRole");
        modelBuilder.HasPostgresEnum<CandidateStatus>(name: "CandidateStatus");

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CandidateConfiguration());
    }
}
