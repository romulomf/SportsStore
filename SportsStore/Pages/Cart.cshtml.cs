using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
	public class CartModel(IStoreRepository repository, Cart cart) : PageModel
	{
		private readonly IStoreRepository repository = repository;

		public Cart Cart { get; set; } = cart;

		public string ReturnUrl { get; set; } = "/";

		public void OnGet(string returnUrl)
		{
			ReturnUrl = returnUrl ?? "/";
			//Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
		}

		public IActionResult OnPost(long productId, string returnUrl)
		{
			Product? product = repository.Products.FirstOrDefault(p => p.ProductId == productId);
			if (product != null)
			{
				Cart.AddItem(product, 1);
			}
			return RedirectToPage(new { returnUrl });
		}

		public IActionResult OnPostRemove(long productId, string returnUrl)
		{
			Cart.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductId == productId).Product);
			return RedirectToPage(new { returnUrl });
		}
	}
}