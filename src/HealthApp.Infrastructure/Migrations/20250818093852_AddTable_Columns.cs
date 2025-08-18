using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HealthApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTable_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "column_taxonomies",
                schema: "healthapp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Type = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_column_taxonomies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "columns",
                schema: "healthapp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Slug = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Summary = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    DisplayImage = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_columns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "column_taxonomy_associations",
                schema: "healthapp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    ColumnId = table.Column<long>(type: "bigint", nullable: false),
                    TaxonomyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_column_taxonomy_associations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_column_taxonomy_associations_column_taxonomies_TaxonomyId",
                        column: x => x.TaxonomyId,
                        principalSchema: "healthapp",
                        principalTable: "column_taxonomies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_column_taxonomy_associations_columns_ColumnId",
                        column: x => x.ColumnId,
                        principalSchema: "healthapp",
                        principalTable: "columns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ux_coltax_type_name",
                schema: "healthapp",
                table: "column_taxonomies",
                columns: new[] { "Type", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_coltaxassoc_columnid",
                schema: "healthapp",
                table: "column_taxonomy_associations",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "ix_coltaxassoc_taxonomyid",
                schema: "healthapp",
                table: "column_taxonomy_associations",
                column: "TaxonomyId");

            migrationBuilder.CreateIndex(
                name: "ux_column_taxonomy_association",
                schema: "healthapp",
                table: "column_taxonomy_associations",
                columns: new[] { "ColumnId", "TaxonomyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_columns_published_isdeleted_publishedat",
                schema: "healthapp",
                table: "columns",
                columns: new[] { "IsPublished", "IsDeleted", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "ux_columns_slug",
                schema: "healthapp",
                table: "columns",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "column_taxonomy_associations",
                schema: "healthapp");

            migrationBuilder.DropTable(
                name: "column_taxonomies",
                schema: "healthapp");

            migrationBuilder.DropTable(
                name: "columns",
                schema: "healthapp");
        }
    }
}
