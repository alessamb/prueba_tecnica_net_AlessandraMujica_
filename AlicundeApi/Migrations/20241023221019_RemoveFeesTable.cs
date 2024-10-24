using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlicundeApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFeesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar todos los registros de la tabla Fees
            migrationBuilder.Sql("DELETE FROM Fees;");

            
            migrationBuilder.Sql("DBCC CHECKIDENT ('Fees', RESEED, 0);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropTable(
                name: "Fees");

            
            migrationBuilder.CreateTable(
                name: "Fees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                                                                   
                    Name = table.Column<string>(nullable: true), 
                    Amount = table.Column<decimal>(nullable: false) 
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fees", x => x.Id);
                });
        }
    }
}
