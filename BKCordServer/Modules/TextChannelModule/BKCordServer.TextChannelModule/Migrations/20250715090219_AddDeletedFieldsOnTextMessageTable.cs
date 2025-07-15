using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKCordServer.TextChannelModule.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedFieldsOnTextMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "text_channel",
                table: "text_messages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "text_channel",
                table: "text_messages",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "text_channel",
                table: "text_messages");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "text_channel",
                table: "text_messages");
        }
    }
}
