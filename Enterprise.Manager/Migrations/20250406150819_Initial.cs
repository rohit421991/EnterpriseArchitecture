using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enterprise.Manager.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMPLOYEES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    FIRSTNAME = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    LASTNAME = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    EMAIL = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MOBILE = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ADDRESS = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEES", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMPLOYEES");
        }
    }
}
