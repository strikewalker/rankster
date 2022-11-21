using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankster.Data.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rankster",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrikeUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CostUsd = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Ended = table.Column<bool>(type: "bit", nullable: false),
                    Ending = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rankster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoteTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RankItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RanksterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankItem_Rankster_RanksterId",
                        column: x => x.RanksterId,
                        principalTable: "Rankster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StrikeInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RankItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Paid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vote_RankItem_RankItemId",
                        column: x => x.RankItemId,
                        principalTable: "RankItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vote_VoteTypes_Type",
                        column: x => x.Type,
                        principalTable: "VoteTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "VoteTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { (byte)0, "Up" });

            migrationBuilder.InsertData(
                table: "VoteTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { (byte)1, "Down" });

            migrationBuilder.CreateIndex(
                name: "IX_RankItem_RanksterId",
                table: "RankItem",
                column: "RanksterId");

            migrationBuilder.CreateIndex(
                name: "IX_Rankster_Code",
                table: "Rankster",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vote_RankItemId",
                table: "Vote",
                column: "RankItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_SessionKey",
                table: "Vote",
                column: "SessionKey");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_Type",
                table: "Vote",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "RankItem");

            migrationBuilder.DropTable(
                name: "VoteTypes");

            migrationBuilder.DropTable(
                name: "Rankster");
        }
    }
}
