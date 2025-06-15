namespace TiduPizza.Models;

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<PizzaOrder> PizzaOrders { get; set; }
	public List<OrderBeverage> OrderBeverages { get; set; } = new();

}