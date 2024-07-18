namespace SportsStore.Models
{
	public class EFStoreRepository(StoreDbContext ctx) : IStoreRepository
	{
		private readonly StoreDbContext context = ctx;

		public IQueryable<Product> Products => context.Products;

		public void Create(Product product)
		{
			context.Add(product);
			context.SaveChanges();
		}

		public void Delete(Product product)
		{
			context.Remove(product);
			context.SaveChanges();
		}

		public void Save(Product product) => context.SaveChanges();
	}
}