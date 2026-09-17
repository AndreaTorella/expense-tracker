using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionHouseholdOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AspNetUsers_CreatedByUserId",
                table: "Transaction");

            // 1. Aggiungo HouseholdId temporaneamente nullable
            migrationBuilder.AddColumn<int>(
                name: "HouseholdId",
                table: "Transaction",
                type: "int",
                nullable: true);

            // 2. Popolo HouseholdId delle Transaction già esistenti
            // usando la Household dell'utente che le aveva create
            migrationBuilder.Sql("""
                UPDATE t
                SET t.HouseholdId = u.HouseholdId
                FROM [Transaction] t
                INNER JOIN AspNetUsers u
                    ON t.CreatedByUserId = u.Id
                """);

            // 3. Adesso che i dati sono valorizzati,
            // rendo HouseholdId obbligatorio
            migrationBuilder.AlterColumn<int>(
                name: "HouseholdId",
                table: "Transaction",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // 4. Creo indice
            migrationBuilder.CreateIndex(
                name: "IX_Transaction_HouseholdId",
                table: "Transaction",
                column: "HouseholdId");

            // 5. Ricreo la FK CreatedByUser con Restrict
            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AspNetUsers_CreatedByUserId",
                table: "Transaction",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // 6. Creo la nuova FK verso Household
            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Households_HouseholdId",
                table: "Transaction",
                column: "HouseholdId",
                principalTable: "Households",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AspNetUsers_CreatedByUserId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Households_HouseholdId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_HouseholdId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "HouseholdId",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AspNetUsers_CreatedByUserId",
                table: "Transaction",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
