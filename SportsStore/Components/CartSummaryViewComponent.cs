using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
	public class CartSummaryViewComponent(Cart cart) : ViewComponent
	{
		private readonly Cart cart = cart;

		public IViewComponentResult Invoke()
		{
			return View(cart);
		}
	}
}