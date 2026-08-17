using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrackManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dsps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dsps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    Isrc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracks_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackDistributions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackId = table.Column<int>(type: "int", nullable: false),
                    DspId = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackDistributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackDistributions_Dsps_DspId",
                        column: x => x.DspId,
                        principalTable: "Dsps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackDistributions_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Country", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "Egypt", "amr@music.com", "Amr Diab" },
                    { 2, "Egypt", "angham@music.com", "Angham" },
                    { 3, "Egypt", "info@cairokee.com", "Cairokee" }
                });

            migrationBuilder.InsertData(
                table: "Dsps",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Spotify" },
                    { 2, "Apple Music" },
                    { 3, "YouTube Music" }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "ArtistId", "Genre", "Isrc", "ReleaseDate", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Pop", "EGX000000001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Nour El Ain" },
                    { 2, 1, "Pop", "EGX000000002", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Tamally Maak" },
                    { 3, 3, "Rock", "EGX000000003", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Sot El Horeya" },
                    { 4, 1, "Pop", "EGX000000004", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Ya Ana Ya La" },
                    { 5, 2, "Pop", "EGX000000005", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Sidi Wesalak" },
                    { 6, 3, "Rock", "EGX000000006", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Khatam Solaiman" },
                    { 7, 2, "Classical", "EGX000000007", new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Omri Kabeer" },
                    { 8, 3, "Rock", "EGX000000008", new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Basrah W Ahooh" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackDistributions_DspId",
                table: "TrackDistributions",
                column: "DspId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackDistributions_TrackId_DspId",
                table: "TrackDistributions",
                columns: new[] { "TrackId", "DspId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_ArtistId",
                table: "Tracks",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_Isrc",
                table: "Tracks",
                column: "Isrc",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackDistributions");

            migrationBuilder.DropTable(
                name: "Dsps");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
