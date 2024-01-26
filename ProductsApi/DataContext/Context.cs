using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;

namespace ProductsApi.DataContext
{
	public class Context:IdentityDbContext<AppUser,AppRole,int>
	{
		public Context(DbContextOptions<Context> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Product>().HasData(new Product() { ProductId = 1, ProductName = "Iphone 14", Price = 6000, IsActive = true });
			modelBuilder.Entity<Product>().HasData(new Product() { ProductId = 2, ProductName = "Iphone 15", Price = 7000, IsActive = true }); 
			modelBuilder.Entity<Product>().HasData(new Product() { ProductId = 3, ProductName = "Iphone 16", Price = 11500, IsActive = true });
			modelBuilder.Entity<Product>().HasData(new Product() { ProductId = 4, ProductName = "Iphone 17", Price = 13250, IsActive = true });
			modelBuilder.Entity<Product>().HasData(new Product() { ProductId = 5, ProductName = "Iphone 18", Price = 17250, IsActive = true });
		}

		public DbSet<Product>Products { get; set; }
	}
}
