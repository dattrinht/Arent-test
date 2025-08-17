using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTable_Diaries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "diaries",
                schema: "healthapp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Content = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Preview = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    WrittenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diaries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bodyrecord_profileid_isdeleted_writtenat",
                schema: "healthapp",
                table: "diaries",
                columns: new[] { "ProfileId", "IsDeleted", "WrittenAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "diaries",
                schema: "healthapp");
        }
    }
}
