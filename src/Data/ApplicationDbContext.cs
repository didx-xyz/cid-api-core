using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.TestResult;
using CoviIDApiCore.V1.DTOs.Wallet;
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
        public DbSet<WalletTestResult> WalletTestResults { get; set; }
        public DbSet<WalletDetail> WalletDetails { get; set; }


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
                 v => (V1.DTOs.Credentials.Laboratory)Enum.Parse(typeof(V1.DTOs.Credentials.Laboratory), v)
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
               .Entity<WalletTestResult>()
               .Property(e => e.Laboratory)
               .HasConversion(
                   v => v.ToString().ToLower(),
                   v => (V1.DTOs.TestResult.Laboratory)Enum.Parse(typeof(V1.DTOs.TestResult.Laboratory), v)
               );
            modelBuilder
               .Entity<WalletTestResult>()
               .Property(e => e.ResultStatus)
               .HasConversion(
                   v => v.ToString().ToLower(),
                   v => (ResultStatus)Enum.Parse(typeof(ResultStatus), v)
               );
            modelBuilder
             .Entity<WalletTestResult>()
             .Property(e => e.LaboratoryStatus)
             .HasConversion(
                 v => v.ToString().ToLower(),
                 v => (LaboratoryStatus)Enum.Parse(typeof(LaboratoryStatus), v)
             );

            modelBuilder
             .Entity<WalletDetail>()
             .Property(e => e.IdType)
             .HasConversion(
                 v => v.ToString().ToLower(),
                 v => (IdType)Enum.Parse(typeof(IdType), v)
             );
        }
    }
}