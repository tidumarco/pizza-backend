namespace TiduPizza.Models.DTOs
{
    public class PizzaOrderDTO
    {
        public int PizzaId { get; set; }
        public PizzaDTO Pizza { get; set; } = null!;
        public int OrderId { get; set; }
        public OrderDTO Order { get; set; } = null!;
    }
}
