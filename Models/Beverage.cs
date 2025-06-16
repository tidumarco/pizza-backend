namespace TiduPizza.Models
{
    public class Beverage
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<OrderBeverage> OrderBeverages { get; set; } = new();
    }
}
