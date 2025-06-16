namespace TiduPizza.Models
{
    public class OrderBeverage
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int BeverageId { get; set; }
        public Beverage Beverage { get; set; }
    }
}
