using CoviIDApiCore.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace CoviIDApiCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<OrganisationCounter> OrganisationCounters { get; set; }
        public DbSet<OtpToken> OtpTokens { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}