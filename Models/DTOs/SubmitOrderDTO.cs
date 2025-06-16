namespace TiduPizza.Models.DTOs
{
    public class SubmitOrderDto
    {
        public List<int> PizzaIds { get; set; } = new();
		public List<int> BeverageIds { get; set; } = new();
	}
}
