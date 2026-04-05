using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShivamFinlytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoriesAndTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Salary", 1 },
                    { 2, "Food & Groceries", 2 },
                    { 3, "Investments", 1 }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "Amount", "CategoryId", "CreatedAt", "Date", "IsDeleted", "Note", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, 5000.00m, 1, new DateTime(2026, 4, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Monthly Salary Deposit", "Income", 1 },
                    { 2, 45.50m, 2, new DateTime(2026, 4, 3, 13, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Lunch at Cafe", "Expense", 1 },
                    { 3, 120.00m, 2, new DateTime(2026, 4, 4, 9, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Weekly Groceries", "Expense", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);
        }
    }
}
