using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevaniPaymentGateway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MerchantIPAddress",
                table: "Transaction",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 3, 12, 12, 57, 22, 934, DateTimeKind.Local).AddTicks(5601),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 12, 12, 37, 56, 26, DateTimeKind.Local).AddTicks(1424));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 3, 12, 12, 57, 22, 920, DateTimeKind.Local).AddTicks(8544),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 12, 12, 37, 55, 980, DateTimeKind.Local).AddTicks(4741));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Merchant",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 3, 12, 12, 57, 22, 881, DateTimeKind.Local).AddTicks(1792),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 12, 12, 37, 55, 907, DateTimeKind.Local).AddTicks(6558));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 3, 12, 12, 57, 22, 938, DateTimeKind.Local).AddTicks(2126),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 12, 12, 37, 56, 36, DateTimeKind.Local).AddTicks(9668));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MerchantIPAddress",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Merchant");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 12, 37, 56, 26, DateTimeKind.Local).AddTicks(1424),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 3, 12, 12, 57, 22, 934, DateTimeKind.Local).AddTicks(5601));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 12, 37, 55, 980, DateTimeKind.Local).AddTicks(4741),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 3, 12, 12, 57, 22, 920, DateTimeKind.Local).AddTicks(8544));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 12, 37, 55, 907, DateTimeKind.Local).AddTicks(6558),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 3, 12, 12, 57, 22, 881, DateTimeKind.Local).AddTicks(1792));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 12, 37, 56, 36, DateTimeKind.Local).AddTicks(9668),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 3, 12, 12, 57, 22, 938, DateTimeKind.Local).AddTicks(2126));
        }
    }
}
