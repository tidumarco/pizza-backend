using Microsoft.AspNetCore.Mvc;
using TiduPizza.Models.DTOs;
using TiduPizza.Services;

namespace TiduPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class BeverageController : ControllerBase
{
    private readonly BeverageService _beverageService;

    public BeverageController(BeverageService beverageService)
    {
        _beverageService = beverageService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BeverageDTO>> GetAll()
    {
        var beverages = _beverageService.GetAll();
        return Ok(beverages);
    }
}
