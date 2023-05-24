using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
    public partial class auth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedOn",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedOn",
                table: "Authors",
                newName: "LastUpdateOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdateOn",
                table: "Authors",
                newName: "LastUpdatedOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedOn",
                table: "Categories",
                type: "datetime2",
                nullable: true);
        }
    }
}
