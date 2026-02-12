using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevaniPaymentGateway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionReference",
                table: "PayAgencyTransaction",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 482, DateTimeKind.Local).AddTicks(2710),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 13, 45, 45, 786, DateTimeKind.Local).AddTicks(1060));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 466, DateTimeKind.Local).AddTicks(6421),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 13, 45, 45, 773, DateTimeKind.Local).AddTicks(7298));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 425, DateTimeKind.Local).AddTicks(7398),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 13, 45, 45, 740, DateTimeKind.Local).AddTicks(833));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 486, DateTimeKind.Local).AddTicks(673),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 13, 45, 45, 789, DateTimeKind.Local).AddTicks(1038));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionReference",
                table: "PayAgencyTransaction");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 13, 45, 45, 786, DateTimeKind.Local).AddTicks(1060),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 482, DateTimeKind.Local).AddTicks(2710));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 13, 45, 45, 773, DateTimeKind.Local).AddTicks(7298),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 466, DateTimeKind.Local).AddTicks(6421));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 13, 45, 45, 740, DateTimeKind.Local).AddTicks(833),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 425, DateTimeKind.Local).AddTicks(7398));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 13, 45, 45, 789, DateTimeKind.Local).AddTicks(1038),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 486, DateTimeKind.Local).AddTicks(673));
        }
    }
}
