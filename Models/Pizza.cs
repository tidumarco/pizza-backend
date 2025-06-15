using System.ComponentModel.DataAnnotations;

namespace TiduPizza.Models;

public class Pizza
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    public int? SauceId { get; set; }
    public Sauce? Sauce { get; set; }
	public decimal Price { get; set; }
	public ICollection<Topping>? Toppings { get; set; }
    public ICollection<PizzaOrder>? PizzaOrders { get; set; }
}