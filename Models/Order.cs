namespace ContosoPizza.Models;

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<PizzaOrder> PizzaOrders { get; set; }
}