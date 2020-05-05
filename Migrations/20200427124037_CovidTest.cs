using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoviIDApiCore.Migrations
{
    public partial class CovidTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CovidTests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WalletId = table.Column<string>(nullable: true),
                    PermissionGrantedAt = table.Column<DateTime>(nullable: false),
                    CredentialIndicator = table.Column<string>(nullable: false),
                    DateTested = table.Column<DateTime>(nullable: false),
                    CovidStatus = table.Column<string>(nullable: false),
                    Laboratory = table.Column<string>(nullable: false),
                    ReferenceNumber = table.Column<string>(nullable: true),
                    HasConsent = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CovidTests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CovidTests");
        }
    }
}
