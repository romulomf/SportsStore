namespace SportsStore.Models
{
	public interface IStoreRepository
	{
		IQueryable<Product> Products { get; }

		void Save(Product product);

		void Create(Product product);

		void Delete(Product product);
	}
}