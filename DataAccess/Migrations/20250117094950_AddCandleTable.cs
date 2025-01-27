using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCandleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpenTime = table.Column<long>(type: "bigint", nullable: false),
                    Open = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    Close = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candles");
        }
    }
}
