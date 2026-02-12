using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevaniPaymentGateway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "PayAgencyTransaction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 855, DateTimeKind.Local).AddTicks(1823),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 482, DateTimeKind.Local).AddTicks(2710));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 818, DateTimeKind.Local).AddTicks(6708),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 466, DateTimeKind.Local).AddTicks(6421));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 752, DateTimeKind.Local).AddTicks(2428),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 425, DateTimeKind.Local).AddTicks(7398));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 864, DateTimeKind.Local).AddTicks(9313),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 486, DateTimeKind.Local).AddTicks(673));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "PayAgencyTransaction");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateGenerated",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 482, DateTimeKind.Local).AddTicks(2710),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 855, DateTimeKind.Local).AddTicks(1823));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Merchant",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 466, DateTimeKind.Local).AddTicks(6421),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 818, DateTimeKind.Local).AddTicks(6708));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditTrail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 425, DateTimeKind.Local).AddTicks(7398),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 752, DateTimeKind.Local).AddTicks(2428));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 10, 15, 55, 10, 486, DateTimeKind.Local).AddTicks(673),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 11, 11, 8, 15, 864, DateTimeKind.Local).AddTicks(9313));
        }
    }
}
