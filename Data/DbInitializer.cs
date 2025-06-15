using TiduPizza.Models;

namespace TiduPizza.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PizzaContext context)
        {
            if (context.Pizzas.Any() && context.Toppings.Any() && context.Sauces.Any())
            {
                return;
            }

            var mozzarella = new Topping { Name = "Mozzarella di Bufala", Calories = 85 };
            var basilico = new Topping { Name = "basilico", Calories = 1 };
            var aglio = new Topping { Name = "Aglio", Calories = 4 };
            var origano = new Topping { Name = "Origano", Calories = 2 };
            var acciughe = new Topping { Name = "Acciughe", Calories = 40 };
            var peperoni = new Topping { Name = "Peperoni", Calories = 120 };
            var prosciutto = new Topping { Name = "Prosciutto Crudo", Calories = 90 };
            var funghi = new Topping { Name = "Funghi", Calories = 15 };
            var rucola = new Topping { Name = "Rucola", Calories = 5 };
            var parmigiano = new Topping { Name = "Parmigiano Reggiano", Calories = 110 };

            context.Toppings.AddRange(mozzarella, basilico, aglio, origano, acciughe, peperoni, prosciutto, funghi, rucola, parmigiano);

            var tomato = new Sauce { Name = "Salsa di pomodoro", IsVegan = true };
            var whiteBase = new Sauce { Name = "Base olio di oliva", IsVegan = true };

            context.Sauces.AddRange(tomato, whiteBase);


			var pizzas = new Pizza[]
			 {
                 new()
                 {
                 Name = "Margherita",
                 Sauce = tomato,
                 Toppings = new List<Topping> { mozzarella, basilico },
                 Price = 7.50m
                 },
                 new()
                 {
                 Name = "Marinara",
                 Sauce = tomato,
                 Toppings = new List<Topping> { aglio, origano },
                 Price = 6.00m
                 },
                 new()
                 {
                 Name = "Diavola",
                 Sauce = tomato,
                 Toppings = new List<Topping> { mozzarella, peperoni },
                 Price = 8.50m
                 },
                 new()
                 {
                 Name = "Prosciutto e Funghi",
                 Sauce = tomato,
                 Toppings = new List<Topping> { mozzarella, prosciutto, funghi },
                 Price = 9.00m
                 },
                 new()
                 {
                 Name = "Parmigiana",
                 Sauce = tomato,
                 Toppings = new List<Topping> { mozzarella, parmigiano, basilico },
                 Price = 8.00m
                 },
                 new()
                 {
                 Name = "Bianca con Rucola",
                 Sauce = whiteBase,
                 Toppings = new List<Topping> { mozzarella, rucola, parmigiano },
                 Price = 8.50m
                 }
			 };



			var beverages = new Beverage[]
			 {
                 new() { Name = "Birra Chiara", Price = 3.50m },
                 new() { Name = "Birra Scura", Price = 4.00m },
                 new() { Name = "Coca Cola", Price = 2.50m }
			 };

			context.Beverages.AddRange(beverages);


			context.Pizzas.AddRange(pizzas);
            context.SaveChanges();
        }
    }
}
