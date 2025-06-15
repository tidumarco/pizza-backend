namespace TiduPizza.Models.DTOs
{
    public class AddItemToOrderDto
    {
        public int ItemId { get; set; }
        public int? OrderId { get; set; }
        public string ItemType { get; set; } = "pizza"; // "pizza" or "beverage"
    }
}
