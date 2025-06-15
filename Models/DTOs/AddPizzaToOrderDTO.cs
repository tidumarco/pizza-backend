namespace TiduPizza.Models.DTOs
{
    public class AddPizzaToOrderDto
    {
        public int PizzaId { get; set; }
        public int? OrderId { get; set; }
    }
}
