using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevaniPaymentGateway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentGateway",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transaction",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MPGSTransaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Merchant",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 13, 16, 54, 51, 764, DateTimeKind.Local).AddTicks(6465),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 12, 12, 32, 9, 827, DateTimeKind.Local).AddTicks(4588));

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "GratipTransaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 13, 16, 54, 51, 750, DateTimeKind.Local).AddTicks(4388),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 12, 12, 32, 9, 812, DateTimeKind.Local).AddTicks(2040));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 13, 16, 54, 51, 781, DateTimeKind.Local).AddTicks(9376),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 12, 12, 32, 9, 844, DateTimeKind.Local).AddTicks(2776));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Transaction",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentGateway",
                table: "Transaction",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transaction",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "MPGSTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Merchant",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 12, 12, 32, 9, 827, DateTimeKind.Local).AddTicks(4588),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 13, 16, 54, 51, 764, DateTimeKind.Local).AddTicks(6465));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "GratipTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 12, 12, 32, 9, 812, DateTimeKind.Local).AddTicks(2040),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 13, 16, 54, 51, 750, DateTimeKind.Local).AddTicks(4388));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 12, 12, 32, 9, 844, DateTimeKind.Local).AddTicks(2776),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 13, 16, 54, 51, 781, DateTimeKind.Local).AddTicks(9376));
        }
    }
}
