using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKCordServer.ServerModule.Migrations
{
    /// <inheritdoc />
    public partial class ChandeRoleMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_role_members",
                schema: "server",
                table: "role_members");

            migrationBuilder.DropColumn(
                name: "ServerId",
                schema: "server",
                table: "role_members");

            migrationBuilder.DropColumn(
                name: "GivenBy",
                schema: "server",
                table: "role_members");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_members",
                schema: "server",
                table: "role_members",
                columns: new[] { "UserId", "RoleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_role_members",
                schema: "server",
                table: "role_members");

            migrationBuilder.AddColumn<Guid>(
                name: "ServerId",
                schema: "server",
                table: "role_members",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GivenBy",
                schema: "server",
                table: "role_members",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_members",
                schema: "server",
                table: "role_members",
                columns: new[] { "UserId", "RoleId", "ServerId" });
        }
    }
}
