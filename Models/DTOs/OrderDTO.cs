namespace TiduPizza.Models.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PizzaDTO> PizzaOrders { get; set; } = new();
        public List<BeverageDTO> Beverages { get; set; } = new();

    }
}
