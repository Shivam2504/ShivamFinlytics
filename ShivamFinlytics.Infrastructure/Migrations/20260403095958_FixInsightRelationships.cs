using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShivamFinlytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixInsightRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalystInsights_Users_UserId",
                table: "AnalystInsights");

            migrationBuilder.DropIndex(
                name: "IX_AnalystInsights_UserId",
                table: "AnalystInsights");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AnalystInsights");

            migrationBuilder.CreateIndex(
                name: "IX_AnalystInsights_CreatedBy",
                table: "AnalystInsights",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalystInsights_Users_CreatedBy",
                table: "AnalystInsights",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalystInsights_Users_CreatedBy",
                table: "AnalystInsights");

            migrationBuilder.DropIndex(
                name: "IX_AnalystInsights_CreatedBy",
                table: "AnalystInsights");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AnalystInsights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnalystInsights_UserId",
                table: "AnalystInsights",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalystInsights_Users_UserId",
                table: "AnalystInsights",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
