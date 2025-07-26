using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoItBetterCoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDeletedToTodoTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TodoTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TodoSubtasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TodoTasks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TodoSubtasks");
        }
    }
}
