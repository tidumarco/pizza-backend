using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models.DTOs;

namespace ContosoPizza.Services;

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

        bool alreadyExists = _context.PizzaOrders
            .Any(po => po.OrderId == order.Id && po.PizzaId == pizzaId);

        if (alreadyExists)
        {
            throw new InvalidOperationException("Pizza is already in this order");
        }

        var pizzaOrder = new PizzaOrder
        {
            PizzaId = pizzaId,
            OrderId = order.Id
        };

        _context.PizzaOrders.Add(pizzaOrder);
        _context.SaveChanges();

        return _context.Orders
            .Include(o => o.PizzaOrders)
                .ThenInclude(po => po.Pizza)
            .First(o => o.Id == order.Id);
    }


    public void RemovePizzaFromOrder(int orderId, int pizzaId)
    {
        var pizzaOrder = _context.PizzaOrders
            .FirstOrDefault(po => po.OrderId == orderId && po.PizzaId == pizzaId)
            ?? throw new InvalidOperationException("Pizza not found in order");

        _context.PizzaOrders.Remove(pizzaOrder);
        _context.SaveChanges();
    }

    public Order SubmitOrder(List<int> pizzaIds)
    {
        if (pizzaIds == null || !pizzaIds.Any())
        {
            throw new InvalidOperationException("No pizzas selected for the order.");
        }

        var pizzas = _context.Pizzas
            .Where(p => pizzaIds.Contains(p.Id))
            .ToList();

        if (pizzas.Count != pizzaIds.Count)
        {
            throw new InvalidOperationException("One or more pizzas not found.");
        }

        var order = new Order
        {
            Name = $"Order {DateTime.UtcNow:yyyyMMdd-HHmmss}",
            CreatedAt = DateTime.UtcNow,
            PizzaOrders = pizzaIds.Select(pizzaId => new PizzaOrder
            {
                PizzaId = pizzaId
            }).ToList()
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        return order;
    }

}
