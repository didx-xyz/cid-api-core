using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.DTOs.Credentials;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoviIDApiCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<OrganisationCounter> OrganisationCounters { get; set; }
        public DbSet<OtpToken> OtpTokens { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<CovidTest> CovidTests { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConvertEnumsToString(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void ConvertEnumsToString(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<CovidTest>()
               .Property(e => e.Laboratory)
               .HasConversion(
                   v => v.ToString().ToLower(),
                   v => (Laboratory)Enum.Parse(typeof(Laboratory), v)
               );

            modelBuilder
               .Entity<CovidTest>()
               .Property(e => e.CovidStatus)
               .HasConversion(
                   v => v.ToString().ToLower(),
                   v => (CovidStatus)Enum.Parse(typeof(CovidStatus), v)
               );

            modelBuilder
             .Entity<CovidTest>()
             .Property(e => e.CredentialIndicator)
             .HasConversion(
                 v => v.ToString().ToLower(),
                 v => (CredentialIndicator)Enum.Parse(typeof(CredentialIndicator), v)
             );

            modelBuilder
                .Entity<OrganisationCounter>()
                .Property(e => e.ScanType)
                .HasConversion(
                    v => v.ToString().ToLower(),
                    v => (ScanType) Enum.Parse(typeof(ScanType), v)
                );
        }
    }
}