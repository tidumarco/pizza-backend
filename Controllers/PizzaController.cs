using TiduPizza.Services;
using TiduPizza.Models;
using Microsoft.AspNetCore.Mvc;
using TiduPizza.Models.DTOs;

namespace TiduPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    readonly PizzaService _service;

    public PizzaController(PizzaService service)
    {
        _service = service;
    }

    [HttpGet]
    public IEnumerable<PizzaDTO> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<PizzaDTO> GetById(int id)
    {
        var pizza = _service.GetById(id);

        if (pizza is not null)
        {
            return pizza;
        }
        else
        {
            return NotFound();
        }
    }

    //[HttpPost]
    //public IActionResult Create(Pizza newPizza)
    //{
    //    var pizza = _service.Create(newPizza);
    //    return CreatedAtAction(nameof(GetById), new { id = pizza!.Id }, pizza);
    //}

    [HttpPut("{id}/addtopping")]
    public IActionResult AddTopping(int id, int toppingId)
    {
        var pizzaToUpdate = _service.GetById(id);

        if (pizzaToUpdate is not null)
        {
            _service.AddTopping(id, toppingId);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPut("{id}/updatesauce")]
    public IActionResult UpdateSauce(int id, int sauceId)
    {
        var pizzaToUpdate = _service.GetById(id);

        if (pizzaToUpdate is not null)
        {
            _service.UpdateSauce(id, sauceId);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("Toppings")]
    public IEnumerable<Topping> GetAllToppings()
    {
        return _service.GetAllToppings();
    }

    [HttpGet("Sauces")]
    public IEnumerable<Sauce> GetAllSauces()
    {
        return _service.GetAllSauces();
    }
}