using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SportsStore.Models;

namespace SportsStore.Components
{
	public class NavigationMenuViewComponent(IStoreRepository repository) : ViewComponent
	{
		private readonly IStoreRepository repository = repository;

		public IViewComponentResult Invoke()
		{
			ViewBag.SelectedCategory = RouteData?.Values["category"];
			return View(repository.Products.Select(p => p.Category).Distinct().OrderBy(p => p));
		}
	}
}
