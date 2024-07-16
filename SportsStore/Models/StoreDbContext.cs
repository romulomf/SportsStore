using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
	public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
	{
		public DbSet<Product> Products => Set<Product>();

		public DbSet<Order> Orders => Set<Order>();
    }
}