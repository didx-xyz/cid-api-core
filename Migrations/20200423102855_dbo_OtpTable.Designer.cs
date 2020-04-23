﻿// <auto-generated />
using System;
using CoviIDApiCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoviIDApiCore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200423102855_dbo_OtpTable")]
    partial class dbo_OtpTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoviIDApiCore.Models.Database.Organisation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name");

                    b.Property<string>("Payload");

                    b.HasKey("Id");

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("CoviIDApiCore.Models.Database.OrganisationCounter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Balance");

                    b.Property<DateTime>("Date");

                    b.Property<string>("DeviceIdentifier");

                    b.Property<int>("Movement");

                    b.Property<Guid?>("OrganisationId");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.ToTable("OrganisationCounters");
                });

            modelBuilder.Entity("CoviIDApiCore.Models.Database.OrganisationCounter", b =>
                {
                    b.HasOne("CoviIDApiCore.Models.Database.Organisation", "Organisation")
                        .WithMany("Counter")
                        .HasForeignKey("OrganisationId");
                });
#pragma warning restore 612, 618
        }
    }
}
