using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKCordServer.TextChannelModule.Migrations
{
    /// <inheritdoc />
    public partial class InitialTextChannelModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "text_channel");

            migrationBuilder.CreateTable(
                name: "text_channels",
                schema: "text_channel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MessageCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_text_channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "text_messages",
                schema: "text_channel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_text_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_text_messages_text_channels_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "text_channel",
                        principalTable: "text_channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_text_messages_ChannelId",
                schema: "text_channel",
                table: "text_messages",
                column: "ChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "text_messages",
                schema: "text_channel");

            migrationBuilder.DropTable(
                name: "text_channels",
                schema: "text_channel");
        }
    }
}
