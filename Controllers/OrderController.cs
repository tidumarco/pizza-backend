﻿using TiduPizza.Models;
using TiduPizza.Models.DTOs;
using TiduPizza.Services;
using Microsoft.AspNetCore.Mvc;

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

	[HttpPost("item")]
	public IActionResult AddItemToOrder([FromBody] AddItemToOrderDto dto)
	{
		try
		{
			var order = _orderService.AddItemToOrder(dto.ItemId, dto.OrderId, dto.ItemType.ToLower());
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
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("Submit")]
    public IActionResult SubmitOrder([FromBody] SubmitOrderDto dto)
    {
        try
        {
            var order = _orderService.SubmitOrder(dto.PizzaIds, dto.BeverageIds);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
