using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
	public class HomeController(IStoreRepository repository) : Controller
	{
		private IStoreRepository repository = repository;

		public int PageSize = 4;

		public ViewResult Index(string? category, int productPage = 1) => View(new ProductsListViewModel
		{
			Products = repository.Products.Where(p => category == null || p.Category == category)
				.OrderBy(p => p.ProductID)
				.Skip((productPage - 1) * PageSize)
				.Take(PageSize),
			PagingInfo = new PagingInfo
			{
				CurrentPage = productPage,
				ItemsPerPage = PageSize,
				TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(p => p.Category == category).Count()
			},
			CurrentCategory = category
		});
	}
}
