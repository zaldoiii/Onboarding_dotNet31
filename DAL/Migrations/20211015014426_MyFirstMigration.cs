using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authors",
                table: "Authors");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "SoccerTeams");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "SoccerCountries");

            migrationBuilder.RenameIndex(
                name: "IX_Books_TeamId",
                table: "SoccerTeams",
                newName: "IX_SoccerTeams_TeamId");

            migrationBuilder.AddColumn<int>(
                name: "SoccerCountryCountryId",
                table: "SoccerTeams",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoccerTeams",
                table: "SoccerTeams",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoccerCountries",
                table: "SoccerCountries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SoccerTeams_SoccerCountryCountryId",
                table: "SoccerTeams",
                column: "SoccerCountryCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoccerTeams_SoccerCountries_SoccerCountryCountryId",
                table: "SoccerTeams",
                column: "SoccerCountryCountryId",
                principalTable: "SoccerCountries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoccerTeams_SoccerCountries_SoccerCountryCountryId",
                table: "SoccerTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoccerTeams",
                table: "SoccerTeams");

            migrationBuilder.DropIndex(
                name: "IX_SoccerTeams_SoccerCountryCountryId",
                table: "SoccerTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoccerCountries",
                table: "SoccerCountries");

            migrationBuilder.DropColumn(
                name: "SoccerCountryCountryId",
                table: "SoccerTeams");

            migrationBuilder.RenameTable(
                name: "SoccerTeams",
                newName: "Books");

            migrationBuilder.RenameTable(
                name: "SoccerCountries",
                newName: "Authors");

            migrationBuilder.RenameIndex(
                name: "IX_SoccerTeams_TeamId",
                table: "Books",
                newName: "IX_Books_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authors",
                table: "Authors",
                column: "CountryId");
        }
    }
}
