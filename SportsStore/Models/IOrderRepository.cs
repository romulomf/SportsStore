namespace SportsStore.Models
{
	public interface IOrderRepository
	{
		public IQueryable<Order> Orders { get; }

		void SaveOrder(Order order);
	}
}