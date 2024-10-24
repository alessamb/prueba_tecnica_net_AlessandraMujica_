using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlicundeApi.Migrations
{
    /// <inheritdoc />
    public partial class ClearFeesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar todos los registros de la tabla Fees
            migrationBuilder.Sql("DELETE FROM Fees;");

            
            migrationBuilder.Sql("DBCC CHECKIDENT ('Fees', RESEED, 0);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
