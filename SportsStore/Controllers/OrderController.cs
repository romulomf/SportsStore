using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
	public class OrderController(IOrderRepository repository, Cart cart) : Controller
	{
		private readonly IOrderRepository repository = repository;

		private readonly Cart cart = cart;

		public ViewResult Checkout() => View(new Order());

		[HttpPost]
		public IActionResult Checkout(Order order)
		{
			if (cart.Lines.Count == 0)
			{
				ModelState.AddModelError("", "Sorry, your cart is empty");
			}
			if (ModelState.IsValid)
			{
				order.Lines = [.. cart.Lines];
				repository.SaveOrder(order);
				cart.Clear();
				return RedirectToPage("/Completed", new { orderId = order.OrderId });
			}
			else
			{
				return View();
			}
		}
	}
}