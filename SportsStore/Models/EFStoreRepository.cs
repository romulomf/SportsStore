namespace SportsStore.Models
{
	public class EFStoreRepository(StoreDbContext ctx) : IStoreRepository
	{
		private StoreDbContext context = ctx;

		public IQueryable<Product> Products => context.Products;
	}
}