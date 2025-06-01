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

        public DbSet<User> Users { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Domain.Entities.Org> Orgs { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UserEmail).IsRequired().HasMaxLength(200);
                entity.HasIndex(u => u.UserEmail).IsUnique();
                entity.Property(u => u.UserPassword).IsRequired().HasColumnType("char(60)");
                entity.Property(u => u.Role).IsRequired().HasConversion<string>().HasMaxLength(50);
                entity.Property(u => u.UserDateCreated)
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.HasOne(u => u.DonorProfile)
                      .WithOne(d => d.User)
                      .HasForeignKey<Donor>(d => d.UserId);

                entity.HasOne(u => u.OrgProfile)
                      .WithOne(o => o.User)
                      .HasForeignKey<Domain.Entities.Org>(o => o.UserId);
            });

            modelBuilder.Entity<Donor>(entity =>
            {
                entity.HasKey(d => d.UserId);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(255);
                entity.Property(d => d.Document).HasMaxLength(50);
                entity.Property(d => d.Phone).HasMaxLength(50);
                entity.Property(d => d.ImageUrl).HasMaxLength(2048);
            });

            modelBuilder.Entity<Domain.Entities.Org>(entity =>
            {
                entity.HasKey(o => o.UserId);
                entity.Property(o => o.OrgName).IsRequired().HasMaxLength(255);
                entity.Property(o => o.Document).HasMaxLength(50);
                entity.Property(o => o.Address).HasMaxLength(500);
                entity.Property(o => o.Description).HasMaxLength(1000);
                entity.Property(o => o.AdminName).HasMaxLength(255);
                entity.Property(o => o.ImageUrl).HasMaxLength(2048);
                entity.Property(o => o.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<Donation>(entity =>
            {
                entity.HasKey(dn => dn.DonationId);
                entity.Property(dn => dn.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
                entity.Property(dn => dn.DonationAmount).HasColumnType("decimal(18, 2)");

                entity.HasOne(dn => dn.Donor)
                      .WithMany(d => d.Donations)
                      .HasForeignKey(dn => dn.DonorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(dn => dn.Org)
                      .WithMany(o => o.Donations)
                      .HasForeignKey(dn => dn.OrgId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(r => r.ReportId);
                entity.HasOne(r => r.Org)
                      .WithMany(o => o.Reports)
                      .HasForeignKey(r => r.OrgId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(255);
                entity.Property(p => p.Address).HasMaxLength(500);
                entity.Property(p => p.Description).HasMaxLength(1000);
                entity.Property(p => p.Image_Url).HasMaxLength(2048);

                entity.HasOne(p => p.Org)
                      .WithMany(o => o.Projects)
                      .HasForeignKey(p => p.OrgId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}