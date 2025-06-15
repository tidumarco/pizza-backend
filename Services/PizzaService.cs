using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService
{
    private readonly PizzaContext _context;

    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    public IEnumerable<Pizza> GetAll()
    {
        return _context.Pizzas
            .Include(p => p.Sauce)
            .Include(p => p.Toppings)
            .Include(p => p.PizzaOrders)
                .ThenInclude(po => po.Order)
            .ToList();
    }


    public Pizza? GetById(int id)
    {
        return _context.Pizzas
            .Include(p => p.Toppings)
            .Include(p => p.Sauce)
            .Include(p => p.PizzaOrders)
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
            // Remove all PizzaOrders entries for this pizza
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
