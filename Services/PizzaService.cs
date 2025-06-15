using TiduPizza.Models;
using TiduPizza.Data;
using Microsoft.EntityFrameworkCore;
using TiduPizza.Models.DTOs;

namespace TiduPizza.Services;

public class PizzaService
{
    private readonly PizzaContext _context;

    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    public IEnumerable<PizzaDTO> GetAll()
    {
        return _context.Pizzas
            .Include(p => p.Sauce)
            .Include(p => p.Toppings)
            .Include(p => p.PizzaOrders)
                .ThenInclude(po => po.Order)
            .Select(pizza => new PizzaDTO
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Sauce = pizza.Sauce == null ? null : new SauceDTO
                {
                    Id = pizza.Sauce.Id,
                    Name = pizza.Sauce.Name
                },
                Toppings = pizza.Toppings.Select(t => new ToppingDTO
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList(),
                PizzaOrders = pizza.PizzaOrders.Select(po => new PizzaOrderDTO
                {
                    OrderId = po.OrderId,
                    Order = new OrderDTO
                    {
                        Id = po.Order.Id,
                        CreatedAt = po.Order.CreatedAt
                    }
                }).ToList(),
                Price = pizza.Price
            })
            .ToList();
    }


    public PizzaDTO? GetById(int id)
    {
        return _context.Pizzas
            .Include(p => p.Toppings)
            .Include(p => p.Sauce)
            .Include(p => p.PizzaOrders)
            .Select(pizza => new PizzaDTO
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Sauce = pizza.Sauce == null ? null : new SauceDTO
                {
                    Id = pizza.Sauce.Id,
                    Name = pizza.Sauce.Name
                },
                Toppings = pizza.Toppings.Select(t => new ToppingDTO
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList(),
                PizzaOrders = pizza.PizzaOrders.Select(po => new PizzaOrderDTO
                {
                    OrderId = po.OrderId,
                    Order = new OrderDTO
                    {
                        Id = po.Order.Id,
                        CreatedAt = po.Order.CreatedAt
                    }
                }).ToList(),
				Price = pizza.Price
			})
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == id);
    }

    public Pizza? Create(Pizza newPizza)
    {
        _context.Pizzas.Add(newPizza);
        _context.SaveChanges();
        return newPizza;
    }

    public void AddTopping(int pizzaId, int toppingId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(pizzaId);
        var toppingToAdd = _context.Toppings.Find(toppingId);

        if (pizzaToUpdate is null || toppingToAdd is null)
        {
            throw new InvalidOperationException("Pizza or topping does not exist");
        }

        if (pizzaToUpdate.Toppings is null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
        }

        pizzaToUpdate.Toppings.Add(toppingToAdd);
        _context.SaveChanges();
    }

    public void UpdateSauce(int pizzaId, int sauceId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(pizzaId);
        var sauceToUpdate = _context.Sauces.Find(sauceId);

        if (pizzaToUpdate is null || sauceToUpdate is null)
        {
            throw new InvalidOperationException("Pizza or sauce does not exist");
        }

        pizzaToUpdate.Sauce = sauceToUpdate;
        _context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var pizzaToDelete = _context.Pizzas
            .Include(p => p.PizzaOrders)
            .FirstOrDefault(p => p.Id == id);

        if (pizzaToDelete != null)
        {
            var pizzaOrders = _context.PizzaOrders.Where(po => po.PizzaId == id);
            _context.PizzaOrders.RemoveRange(pizzaOrders);

            _context.Pizzas.Remove(pizzaToDelete);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Topping> GetAllToppings()
    {
        return _context.Toppings
            .AsNoTracking()
            .ToList();
    }

    public IEnumerable<Sauce> GetAllSauces()
    {
        return _context.Sauces
            .AsNoTracking()
            .ToList();
    }
}
