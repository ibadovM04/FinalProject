﻿namespace FinalProject.DTOs
{
	public class FeaturedProductDto
	{
		public Guid ProductId { get; set; }
		public string? Slug { get; set; }
		public string Name { get; set; }
		public string ImageURL { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
	}
}
