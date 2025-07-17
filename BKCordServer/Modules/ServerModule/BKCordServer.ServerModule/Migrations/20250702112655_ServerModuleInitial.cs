using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKCordServer.ServerModule.Migrations
{
    /// <inheritdoc />
    public partial class ServerModuleInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "server");

            migrationBuilder.CreateTable(
                name: "role_members",
                schema: "server",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    GivenBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_members", x => new { x.UserId, x.RoleId, x.ServerId });
                });

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

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "server",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    Hierarchy = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "server_members",
                schema: "server",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_members", x => new { x.UserId, x.ServerId });
                });

            migrationBuilder.CreateTable(
                name: "server_members_history",
                schema: "server",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeftAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RemovedReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RemovedByUserId = table.Column<Guid>(type: "uuid", maxLength: 450, nullable: true),
                    WasBanned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_members_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "servers",
                schema: "server",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    InviteCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_roles_ServerId_Name",
                schema: "server",
                table: "roles",
                columns: new[] { "ServerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_servers_InviteCode",
                schema: "server",
                table: "servers",
                column: "InviteCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_members",
                schema: "server");

            migrationBuilder.DropTable(
                name: "role_permissions",
                schema: "server");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "server");

            migrationBuilder.DropTable(
                name: "server_members",
                schema: "server");

            migrationBuilder.DropTable(
                name: "server_members_history",
                schema: "server");

            migrationBuilder.DropTable(
                name: "servers",
                schema: "server");
        }
    }
}
