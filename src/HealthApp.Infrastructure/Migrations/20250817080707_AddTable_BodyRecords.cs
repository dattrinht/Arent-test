using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTable_BodyRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "body_records",
                schema: "healthapp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    BodyFat = table.Column<float>(type: "real", nullable: false),
                    RecordedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_body_records", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bodyrecord_profileid_isdeleted_recordedat",
                schema: "healthapp",
                table: "body_records",
                columns: new[] { "ProfileId", "IsDeleted", "RecordedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "body_records",
                schema: "healthapp");
        }
    }
}
