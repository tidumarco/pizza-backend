using TiduPizza.Models;
using TiduPizza.Models.DTOs;
using TiduPizza.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TiduPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetAllOrders()
    {
        return Ok(_orderService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<Order> GetOrderById(int id)
    {
        var order = _orderService.GetById(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public IActionResult AddPizzaToOrder([FromBody] AddPizzaToOrderDto dto)
    {
        try
        {
            var order = _orderService.AddPizzaToOrder(dto.PizzaId, dto.OrderId);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var order = _orderService.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            _orderService.DeleteById(id);
            return NoContent();
        }
        catch (SqliteException ex)
        {
            return BadRequest(new { message = "Failed to delete order due to database constraint: " + ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{orderId}/pizza/{pizzaId}")]
    public IActionResult RemovePizzaFromOrder(int orderId, int pizzaId)
    {
        try
        {
            _orderService.RemovePizzaFromOrder(orderId, pizzaId);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("Submit")]
    public IActionResult SubmitOrder([FromBody] SubmitOrderDto dto)
    {
        try
        {
            var order = _orderService.SubmitOrder(dto.PizzaIds);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
