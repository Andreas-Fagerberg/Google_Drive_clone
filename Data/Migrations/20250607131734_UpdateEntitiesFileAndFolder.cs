using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Google_Drive_clone.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesFileAndFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folders_FolderId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_ParentFolderId",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "ParentFolderId",
                table: "Folders");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Folders",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "FolderName",
                table: "Folders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "FolderNameNormalized",
                table: "Folders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "Files",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Files",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileNameNormalized",
                table: "Files",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_FolderNameNormalized_UserId",
                table: "Folders",
                columns: new[] { "FolderNameNormalized", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_FileNameNormalized_UserId",
                table: "Files",
                columns: new[] { "FileNameNormalized", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folders_FolderId",
                table: "Files",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folders_FolderId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Folders_FolderNameNormalized_UserId",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Files_FileNameNormalized_UserId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FolderNameNormalized",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FileNameNormalized",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Folders",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Files",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "FolderName",
                table: "Folders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "ParentFolderId",
                table: "Folders",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "Files",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folders_FolderId",
                table: "Files",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }
    }
}
