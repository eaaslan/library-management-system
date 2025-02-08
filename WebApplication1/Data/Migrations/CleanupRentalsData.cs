using Microsoft.EntityFrameworkCore.Migrations;

public partial class CleanupRentalsData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Reset all books to available
        migrationBuilder.Sql("UPDATE \"Books\" SET \"Available\" = true");
        
        // Delete all existing rentals
        migrationBuilder.Sql("DELETE FROM \"Rentals\"");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Cannot restore deleted data
    }
} 