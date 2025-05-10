using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Domain.Entities.Org> Orgs { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Donor>()
                .HasMany(d => d.Donations)
                .WithOne(dn => dn.Donor)
                .HasForeignKey(dn => dn.DonorId);

            modelBuilder.Entity<Domain.Entities.Org>()
                .HasMany(o => o.Donations)
                .WithOne(dn => dn.Org)
                .HasForeignKey(dn => dn.OrgId);

            modelBuilder.Entity<Domain.Entities.Org>(entity =>
            {
                entity.Property(o => o.OrgDateCreated)
                  .HasColumnType("timestamp")
                  .HasDefaultValueSql("UTC_TIMESTAMP()")
                  .ValueGeneratedOnAdd();

                entity.Property(o => o.OrgFoundationDate)
                      .HasColumnType("date");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.UserEmail)
                      .IsUnique();

                entity.Property(u => u.UserPassword)
                      .HasColumnType("char(60)")
                      .IsRequired();

                entity.Property(u => u.UserDateCreated)
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("UTC_TIMESTAMP()")
                      .ValueGeneratedOnAdd();

                entity.Property(u => u.BirthDate)
                      .HasColumnType("date");
            });

        }
    }
}
