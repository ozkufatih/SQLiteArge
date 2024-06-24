using Microsoft.EntityFrameworkCore;
using Playground.Domain.Entities;

namespace Playground.Data.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        // Tables
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<PortfolioAsset> PortfolioAssets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PortfolioAsset>()
                .HasKey(pa => new { pa.PortfolioId, pa.AssetId });

            modelBuilder.Entity<PortfolioAsset>()
                .HasOne(pa => pa.Portfolio)
                .WithMany(p => p.PortfolioAssets)
                .HasForeignKey(pa => pa.PortfolioId);

            modelBuilder.Entity<PortfolioAsset>()
                .HasOne(pa => pa.Asset)
                .WithMany(a => a.PortfolioAssets)
                .HasForeignKey(pa => pa.AssetId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("Data Source=playground.db");

        }
    }
}
