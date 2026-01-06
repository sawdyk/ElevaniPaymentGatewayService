using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevaniPaymentGateway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 6, 12, 5, 48, 344, DateTimeKind.Local).AddTicks(5988),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 12, 15, 12, 35, 29, 768, DateTimeKind.Local).AddTicks(5845));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 6, 12, 5, 48, 320, DateTimeKind.Local).AddTicks(7495),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 12, 15, 12, 35, 29, 755, DateTimeKind.Local).AddTicks(3676));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 6, 12, 5, 48, 292, DateTimeKind.Local).AddTicks(2499),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 12, 15, 12, 35, 29, 732, DateTimeKind.Local).AddTicks(7444));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 6, 12, 5, 48, 348, DateTimeKind.Local).AddTicks(9696),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 12, 15, 12, 35, 29, 771, DateTimeKind.Local).AddTicks(516));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 15, 12, 35, 29, 768, DateTimeKind.Local).AddTicks(5845),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 1, 6, 12, 5, 48, 344, DateTimeKind.Local).AddTicks(5988));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 15, 12, 35, 29, 755, DateTimeKind.Local).AddTicks(3676),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 1, 6, 12, 5, 48, 320, DateTimeKind.Local).AddTicks(7495));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 15, 12, 35, 29, 732, DateTimeKind.Local).AddTicks(7444),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 1, 6, 12, 5, 48, 292, DateTimeKind.Local).AddTicks(2499));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 15, 12, 35, 29, 771, DateTimeKind.Local).AddTicks(516),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 1, 6, 12, 5, 48, 348, DateTimeKind.Local).AddTicks(9696));
        }
    }
}
