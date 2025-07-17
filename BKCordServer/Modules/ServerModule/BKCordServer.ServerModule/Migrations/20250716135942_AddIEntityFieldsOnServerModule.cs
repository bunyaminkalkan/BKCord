using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKCordServer.ServerModule.Migrations
{
    /// <inheritdoc />
    public partial class AddIEntityFieldsOnServerModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "server",
                table: "servers");

            migrationBuilder.DropColumn(
                name: "JoinedAt",
                schema: "server",
                table: "server_members_history");

            migrationBuilder.RenameColumn(
                name: "LeftAt",
                schema: "server",
                table: "server_members_history",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "server",
                table: "servers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "server",
                table: "servers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "server",
                table: "servers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "server",
                table: "server_members_history",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "server",
                table: "server_members_history",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "server",
                table: "server_members_history",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "server",
                table: "roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "server",
                table: "roles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "server",
                table: "roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "server",
                table: "roles",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "server",
                table: "servers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "server",
                table: "servers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "server",
                table: "server_members_history");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "server",
                table: "server_members_history");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "server",
                table: "server_members_history");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "server",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "server",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "server",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "server",
                table: "roles");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "server",
                table: "server_members_history",
                newName: "LeftAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "server",
                table: "servers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Status",
                schema: "server",
                table: "servers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedAt",
                schema: "server",
                table: "server_members_history",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
