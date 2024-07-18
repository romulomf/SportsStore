using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Xunit;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;

namespace SportsStore.Tests
{
	public class CartPageTests
	{
		[Fact]
		public void Can_Load_Cart()
		{
			// Arrange
			// - create a mock repository
			Product p1 = new() { ProductId = 1, Name = "P1" };
			Product p2 = new() { ProductId = 2, Name = "P2" };
			/**
			 * cria o mock do repositório que simula a devolução dos dois produtos criados anteriormente
			 * quando for solicitada a leitura dos produtos existentes durante este teste.
			 */
			Mock<IStoreRepository> mockRepo = new();
			mockRepo.Setup(m => m.Products).Returns((new Product[] { p1, p2}).AsQueryable<Product>());

			// cria um carrinho para servir de mock para o teste
			Cart testCart = new();
			testCart.AddItem(p1, 2);
			testCart.AddItem(p2, 1);

			// Action
			CartModel cartModel = new(mockRepo.Object, testCart);
			cartModel.OnGet("myUrl");

			// Assert
			Assert.Equal(2, cartModel.Cart?.Lines.Count);
			// verifica se a URL de retorno é a mesma que foi recebida na chamada de OnGet
			Assert.Equal("myUrl", cartModel.ReturnUrl);
		}

		[Fact]
		public void Can_Update_Cart()
		{
			// Arrange
			// - create a mock repository
			Mock<IStoreRepository> mockRepo = new();
			mockRepo.Setup(m => m.Products).Returns((new Product[] {
				new() { ProductId = 1, Name = "P1" }
			}).AsQueryable<Product>());

			Cart? testCart = new();

			// Action
			CartModel cartModel = new(mockRepo.Object, testCart);
			cartModel.OnPost(1, "myUrl");

			//Assert
			Assert.Single(testCart.Lines);
			Assert.Equal("P1", testCart.Lines.First().Product.Name);
			Assert.Equal(1, testCart.Lines.First().Quantity);
		}
	}
}