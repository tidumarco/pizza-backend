namespace ContosoPizza.Models
{
    public class PizzaOrder
    {
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; } = null!;
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
