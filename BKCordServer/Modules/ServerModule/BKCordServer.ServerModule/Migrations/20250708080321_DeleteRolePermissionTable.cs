using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKCordServer.ServerModule.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRolePermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_permissions",
                schema: "server");

            migrationBuilder.AddColumn<string>(
                name: "RolePermissions",
                schema: "server",
                table: "roles",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RolePermissions",
                schema: "server",
                table: "roles");

            migrationBuilder.CreateTable(
                name: "role_permissions",
                schema: "server",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerPermissions = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => x.RoleId);
                });
        }
    }
}
