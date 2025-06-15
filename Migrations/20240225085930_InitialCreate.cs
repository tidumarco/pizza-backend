using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiduPizza.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sauces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sauces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pizzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    SauceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pizzas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pizzas_Sauces_SauceId",
                        column: x => x.SauceId,
                        principalTable: "Sauces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Toppings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: true),
                    OrdersId = table.Column<int>(type: "INTEGER", nullable: true)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toppings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toppings_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });


            migrationBuilder.CreateTable(
             name: "PizzaOrders",
             columns: table => new
             {
                 OrdersId = table.Column<int>(type: "INTEGER", nullable: false),
                 PizzasId = table.Column<int>(type: "INTEGER", nullable: false)
             },
             constraints: table =>
             {
                 table.PrimaryKey("PK_PizzaOrders", x => new { x.OrdersId, x.PizzasId });
                 table.ForeignKey(
                 name: "FK_PizzaOrders_Orders_OrdersId",
                 column: x => x.OrdersId,
                 principalTable: "Orders",
                 principalColumn: "Id",
                 onDelete: ReferentialAction.Cascade);
                             table.ForeignKey(
                 name: "FK_PizzaOrders_Pizzas_PizzasId",
                 column: x => x.PizzasId,
                 principalTable: "Pizzas",
                 principalColumn: "Id",
                 onDelete: ReferentialAction.Cascade);
             });


            migrationBuilder.CreateIndex(
                name: "IX_PizzaOrders_PizzasId",
                table: "PizzaOrders",
                column: "PizzasId");


            migrationBuilder.CreateIndex(
                name: "IX_Pizzas_SauceId",
                table: "Pizzas",
                column: "SauceId");

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_PizzaId",
                table: "Toppings",
                column: "PizzaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Toppings");
            migrationBuilder.DropTable(
                name: "Pizzas");
            migrationBuilder.DropTable(
                name: "Sauces");
            migrationBuilder.DropTable(
                name: "PizzaOrders");
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
