using TiduPizza.Data;
using TiduPizza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TiduPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class CouponController : ControllerBase
{
	readonly PromotionsContext _context;

	public CouponController(PromotionsContext context)
	{
		_context = context;
	}

	[HttpGet]
	public IEnumerable<Coupon> Get()
	{
		return _context.Coupons
		.AsNoTracking()
		.ToList();
	}

}