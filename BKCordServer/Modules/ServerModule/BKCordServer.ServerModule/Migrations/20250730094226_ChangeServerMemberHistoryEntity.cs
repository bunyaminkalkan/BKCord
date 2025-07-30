using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKCordServer.ServerModule.Migrations
{
    /// <inheritdoc />
    public partial class ChangeServerMemberHistoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "server_members_history",
                schema: "server");

            migrationBuilder.CreateTable(
                name: "server_member_history",
                schema: "server",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ActionedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false),
                    IsKicked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_member_history", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "server_member_history",
                schema: "server");

            migrationBuilder.CreateTable(
                name: "server_members_history",
                schema: "server",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RemovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    RemovedReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WasBanned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_members_history", x => x.Id);
                });
        }
    }
}
