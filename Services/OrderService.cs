using TiduPizza.Models;
using TiduPizza.Data;
using Microsoft.EntityFrameworkCore;
using TiduPizza.Models.DTOs;

namespace TiduPizza.Services;

public class OrderService
{
    private readonly PizzaContext _context;

    public OrderService(PizzaContext context)
    {
        _context = context;
    }

    public IEnumerable<OrderDTO> GetAll()
    {
        return _context.Orders
                .Include(o => o.PizzaOrders)
                        .ThenInclude(p => p.Pizza.Toppings)
                .Include(o => o.PizzaOrders)
                        .ThenInclude(p => p.Pizza.Sauce)
                .AsNoTracking()
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    Name = o.Name,
                    CreatedAt = o.CreatedAt,
                    PizzaOrders = o.PizzaOrders.Select(po => new PizzaDTO
                    {
                        Id = po.Pizza.Id,
                        Name = po.Pizza.Name,
                        Sauce = po.Pizza.Sauce == null ? null : new SauceDTO
                        {
                            Id = po.Pizza.Sauce.Id,
                            Name = po.Pizza.Sauce.Name
                        },
                        Toppings = po.Pizza.Toppings.Select(t => new ToppingDTO
                        {
                            Id = t.Id,
                            Name = t.Name
                        }).ToList()
                    }).ToList(),
					Beverages = o.OrderBeverages.Select(ob => new BeverageDTO
					{
						Id = ob.Beverage.Id,
						Name = ob.Beverage.Name,
						Price = ob.Beverage.Price
					}).ToList()


				})
                .ToList();
    }

    public OrderDTO? GetById(int id)
    {
        return _context.Orders
            .Include(o => o.PizzaOrders)
                    .ThenInclude(p => p.Pizza.Toppings)
            .Include(o => o.PizzaOrders)
                    .ThenInclude(p => p.Pizza.Sauce)
            .AsNoTracking()
            .Select(o => new OrderDTO
            {
                Id = o.Id,
                Name = o.Name,
                CreatedAt = o.CreatedAt,
                PizzaOrders = o.PizzaOrders.Select(po => new PizzaDTO
                {
                    Id = po.Pizza.Id,
                    Name = po.Pizza.Name
                }).ToList(),
				Beverages = o.OrderBeverages.Select(ob => new BeverageDTO
				{
					Id = ob.Beverage.Id,
					Name = ob.Beverage.Name,
					Price = ob.Beverage.Price
				}).ToList()
			})
            .SingleOrDefault(o => o.Id == id);
    }


    public Order Create(Order newOrder)
    {
        _context.Orders.Add(newOrder);
        _context.SaveChanges();
        return newOrder;
    }

    public void DeleteById(int id)
    {
        var orderToDelete = _context.Orders
            .Include(o => o.PizzaOrders)
            .FirstOrDefault(o => o.Id == id);

        if (orderToDelete == null)
        {
            return;
        }

        var pizzaOrders = _context.PizzaOrders.Where(po => po.OrderId == id);
        _context.PizzaOrders.RemoveRange(pizzaOrders);

        _context.Orders.Remove(orderToDelete);
        _context.SaveChanges();
    }

	public Order AddItemToOrder(int itemId, int? orderId, string itemType)
	{
		Order order;

		if (orderId.HasValue)
		{
			order = _context.Orders
				.Include(o => o.PizzaOrders)
				.Include(o => o.OrderBeverages)
				.FirstOrDefault(o => o.Id == orderId.Value)
				?? throw new InvalidOperationException("Order not found");
		}
		else
		{
			order = new Order
			{
				Name = $"Order {DateTime.UtcNow:yyyyMMdd-HHmmss}",
				CreatedAt = DateTime.UtcNow,
				PizzaOrders = new List<PizzaOrder>(),
				OrderBeverages = new List<OrderBeverage>()
			};

			_context.Orders.Add(order);
			_context.SaveChanges();
		}

		if (itemType == "pizza")
		{
			var pizza = _context.Pizzas.Find(itemId)
				?? throw new InvalidOperationException("Pizza not found");

			bool alreadyExists = _context.PizzaOrders
				.Any(po => po.OrderId == order.Id && po.PizzaId == itemId);

			if (alreadyExists)
				throw new InvalidOperationException("Pizza is already in this order");

			_context.PizzaOrders.Add(new PizzaOrder { PizzaId = itemId, OrderId = order.Id });
		}
		else if (itemType == "beverage")
		{
			var beverage = _context.Beverages.Find(itemId)
				?? throw new InvalidOperationException("Beverage not found");

			bool alreadyExists = _context.OrderBeverages
				.Any(ob => ob.OrderId == order.Id && ob.BeverageId == itemId);

			if (alreadyExists)
				throw new InvalidOperationException("Beverage is already in this order");

			_context.OrderBeverages.Add(new OrderBeverage { BeverageId = itemId, OrderId = order.Id });
		}

		_context.SaveChanges();

		return _context.Orders
			.Include(o => o.PizzaOrders).ThenInclude(po => po.Pizza)
			.Include(o => o.OrderBeverages).ThenInclude(ob => ob.Beverage)
			.First(o => o.Id == order.Id);
	}



	public void RemoveItemFromOrder(int orderId, int itemId, string itemType)
	{
		if (itemType == "pizza")
		{
			var pizzaOrder = _context.PizzaOrders
				.FirstOrDefault(po => po.OrderId == orderId && po.PizzaId == itemId)
				?? throw new InvalidOperationException("Pizza not found in order");

			_context.PizzaOrders.Remove(pizzaOrder);
		}
		else if (itemType == "beverage")
		{
			var beverageOrder = _context.OrderBeverages
				.FirstOrDefault(ob => ob.OrderId == orderId && ob.BeverageId == itemId)
				?? throw new InvalidOperationException("Beverage not found in order");

			_context.OrderBeverages.Remove(beverageOrder);
		}

		_context.SaveChanges();
	}


	public Order SubmitOrder(List<int> pizzaIds, List<int> beverageIds)
	{
		if ((pizzaIds == null || !pizzaIds.Any()) && (beverageIds == null || !beverageIds.Any()))
		{
			throw new InvalidOperationException("No items selected for the order.");
		}

		var order = new Order
		{
			Name = $"Order {DateTime.UtcNow:yyyyMMdd-HHmmss}",
			CreatedAt = DateTime.UtcNow,
			PizzaOrders = pizzaIds?.Select(id => new PizzaOrder { PizzaId = id }).ToList() ?? new(),
			OrderBeverages = beverageIds?.Select(id => new OrderBeverage { BeverageId = id }).ToList() ?? new()
		};

		_context.Orders.Add(order);
		_context.SaveChanges();

		return order;
	}


}
