using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTable_Meals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "meals",
                schema: "healthapp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Type = table.Column<short>(type: "smallint", nullable: false),
                    Image = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DoneAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_meal_profileid_isdeleted_doneat",
                schema: "healthapp",
                table: "meals",
                columns: new[] { "ProfileId", "IsDeleted", "DoneAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "meals",
                schema: "healthapp");
        }
    }
}
