﻿namespace ProductsApi.Dto
{
	public class ProductDto
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public decimal Price { get; set; }
	}
}
