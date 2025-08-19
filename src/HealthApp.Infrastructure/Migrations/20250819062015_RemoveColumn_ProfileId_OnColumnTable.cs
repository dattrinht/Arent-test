using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumn_ProfileId_OnColumnTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileId",
                schema: "healthapp",
                table: "columns");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                schema: "healthapp",
                table: "column_taxonomy_associations");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                schema: "healthapp",
                table: "column_taxonomies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProfileId",
                schema: "healthapp",
                table: "columns",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProfileId",
                schema: "healthapp",
                table: "column_taxonomy_associations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProfileId",
                schema: "healthapp",
                table: "column_taxonomies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
