using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevaniPaymentGateway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 12, 37, 56, 26, DateTimeKind.Local).AddTicks(1424),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 855, DateTimeKind.Local).AddTicks(1823));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 12, 37, 55, 980, DateTimeKind.Local).AddTicks(4741),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 818, DateTimeKind.Local).AddTicks(6708));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 12, 37, 55, 907, DateTimeKind.Local).AddTicks(6558),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 752, DateTimeKind.Local).AddTicks(2428));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 12, 37, 56, 36, DateTimeKind.Local).AddTicks(9668),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 864, DateTimeKind.Local).AddTicks(9313));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Transaction");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 855, DateTimeKind.Local).AddTicks(1823),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 12, 12, 37, 56, 26, DateTimeKind.Local).AddTicks(1424));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 818, DateTimeKind.Local).AddTicks(6708),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 12, 12, 37, 55, 980, DateTimeKind.Local).AddTicks(4741));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 752, DateTimeKind.Local).AddTicks(2428),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 12, 12, 37, 55, 907, DateTimeKind.Local).AddTicks(6558));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 864, DateTimeKind.Local).AddTicks(9313),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 12, 12, 37, 56, 36, DateTimeKind.Local).AddTicks(9668));
        }
    }
}
