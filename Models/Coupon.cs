﻿using System;
using System.Collections.Generic;

namespace TiduPizza.Models;

public partial class Coupon
{
	public long Id { get; set; }

	public string Description { get; set; } = null!;

	public DateTime Expiration { get; set; }
}
