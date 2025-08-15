using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changestructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Installments_LoanApplications_LoanApplicationId",
                table: "Installments");

            migrationBuilder.DropForeignKey(
                name: "FK_Installments_Loans_LoanId",
                table: "Installments");

            migrationBuilder.DropIndex(
                name: "IX_Installments_LoanId",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "LoanId",
                table: "Installments");

            migrationBuilder.AlterColumn<string>(
                name: "LoanApplicationId",
                table: "Installments",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_LoanApplications_LoanApplicationId",
                table: "Installments",
                column: "LoanApplicationId",
                principalTable: "LoanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Installments_LoanApplications_LoanApplicationId",
                table: "Installments");

            migrationBuilder.AlterColumn<string>(
                name: "LoanApplicationId",
                table: "Installments",
                type: "nvarchar(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36);

            migrationBuilder.AddColumn<string>(
                name: "LoanId",
                table: "Installments",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Installments_LoanId",
                table: "Installments",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_LoanApplications_LoanApplicationId",
                table: "Installments",
                column: "LoanApplicationId",
                principalTable: "LoanApplications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_Loans_LoanId",
                table: "Installments",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
