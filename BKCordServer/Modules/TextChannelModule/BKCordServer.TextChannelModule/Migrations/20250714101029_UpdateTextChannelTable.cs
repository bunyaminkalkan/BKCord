using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKCordServer.TextChannelModule.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTextChannelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "text_channel",
                table: "text_channels",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "text_channel",
                table: "text_channels",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "text_channel",
                table: "text_channels",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "text_channel",
                table: "text_channels");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "text_channel",
                table: "text_channels");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "text_channel",
                table: "text_channels");
        }
    }
}
