using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class OrderService
{
    private readonly PizzaContext _context;

    public OrderService(PizzaContext context)
    {
        _context = context;
    }

    public IEnumerable<Order> GetAll()
    {
        return _context.Orders
            .Include(o => o.PizzaOrders)
                .ThenInclude(po => po.Pizza)
                    .ThenInclude(p => p.Toppings)
            .Include(o => o.PizzaOrders)
                .ThenInclude(po => po.Pizza)
                    .ThenInclude(p => p.Sauce)
            .ToList();
    }


    public Order? GetById(int id)
    {
        return _context.Orders
            .Include(o => o.PizzaOrders)
                .ThenInclude(po => po.Pizza)
                    .ThenInclude(p => p.Toppings)
            .Include(o => o.PizzaOrders)
                .ThenInclude(po => po.Pizza)
                    .ThenInclude(p => p.Sauce)
            .AsNoTracking()
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

        // Remove all PizzaOrders entries for this order
        var pizzaOrders = _context.PizzaOrders.Where(po => po.OrderId == id);
        _context.PizzaOrders.RemoveRange(pizzaOrders);

        _context.Orders.Remove(orderToDelete);
        _context.SaveChanges();
    }

    public Order AddPizzaToOrder(int pizzaId, int? orderId)
    {
        var pizza = _context.Pizzas
            .Include(p => p.Sauce)
            .Include(p => p.Toppings)
            .FirstOrDefault(p => p.Id == pizzaId)
            ?? throw new InvalidOperationException("Pizza not found");

        Order order;
        if (orderId.HasValue)
        {
            order = _context.Orders
                .Include(o => o.PizzaOrders)
                .ThenInclude(po => po.Pizza)
                .FirstOrDefault(o => o.Id == orderId.Value)
                ?? throw new InvalidOperationException("Order not found");
        }
        else
        {
            order = new Order
            {
                Name = $"Order {DateTime.UtcNow:yyyyMMdd-HHmmss}",
                CreatedAt = DateTime.UtcNow,
                PizzaOrders = new List<PizzaOrder>()
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        // Check if the pizza is already in the order
        if (order.PizzaOrders.Any(po => po.PizzaId == pizzaId))
        {
            throw new InvalidOperationException("Pizza is already in this order");
        }

        // Add to PizzaOrders
        var pizzaOrder = new PizzaOrder { PizzaId = pizzaId, OrderId = order.Id };
        _context.PizzaOrders.Add(pizzaOrder);

        _context.SaveChanges();
        return order;
    }

    public void RemovePizzaFromOrder(int orderId, int pizzaId)
    {
        var pizzaOrder = _context.PizzaOrders
            .FirstOrDefault(po => po.OrderId == orderId && po.PizzaId == pizzaId)
            ?? throw new InvalidOperationException("Pizza not found in order");

        _context.PizzaOrders.Remove(pizzaOrder);
        _context.SaveChanges();
    }
}
