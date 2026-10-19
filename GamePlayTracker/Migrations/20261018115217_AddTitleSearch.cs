using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlayTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleSearch",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
                migrationBuilder.Sql("UPDATE Games SET TitleSearch = lower(Title)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleSearch",
                table: "Games");
        }
    }
}
