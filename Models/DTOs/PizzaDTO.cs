using TiduPizza.Models;
using TiduPizza.Models.DTOs;

public class PizzaDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public SauceDTO? Sauce { get; set; }
    public List<ToppingDTO> Toppings { get; set; } = new();
    public List<PizzaOrderDTO> PizzaOrders { get; set; } = new();
}
