using TiduPizza.Models;
using TiduPizza.Data;
using Microsoft.EntityFrameworkCore;
using TiduPizza.Models.DTOs;

namespace TiduPizza.Services;

public class BeverageService
{
    private readonly PizzaContext _context;

    public BeverageService(PizzaContext context)
    {
        _context = context;
    }

    public IEnumerable<BeverageDTO> GetAll()
    {
        return _context.Beverages
.           Include(b => b.OrderBeverages)
                .ThenInclude(ob => ob.Order)
            .Select(Beverage => new BeverageDTO
            {
                Id = Beverage.Id,
                Name = Beverage.Name,
                Price = Beverage.Price
            })
            .ToList();
    }


    public BeverageDTO? GetById(int id)
    {
        return _context.Beverages
            .Include(b => b.OrderBeverages)
                .ThenInclude(ob => ob.Order)
            .Select(Beverage => new BeverageDTO
            {
                Id = Beverage.Id,
                Name = Beverage.Name,
                Price = Beverage.Price
            })
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == id);
    }

    public Beverage? Create(Beverage newBeverage)
    {
        _context.Beverages.Add(newBeverage);
        _context.SaveChanges();
        return newBeverage;
    }

    public void DeleteById(int id)
    {
        var BeverageToDelete = _context.Beverages
            .Include(p => p.OrderBeverages)
            .FirstOrDefault(p => p.Id == id);

        if (BeverageToDelete != null)
        {
            var BeverageOrders = _context.OrderBeverages.Where(po => po.BeverageId == id);
            _context.OrderBeverages.RemoveRange(BeverageOrders);

            _context.Beverages.Remove(BeverageToDelete);
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
