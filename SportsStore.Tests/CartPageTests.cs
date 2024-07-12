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
			Product p1 = new() { ProductID = 1, Name = "P1" };
			Product p2 = new() { ProductID = 2, Name = "P2" };
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
			// cria a representação em JSON do carrinho testCart que é o mock do teste
			byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(testCart));
			Mock<ISession> mockSession = new();
			/**
			 * quando tentar pegar qualquer chave na sessão, retorna os dados que são representados
			 * pelo carrinho testCart que foi mockado anteriormente.
			 */
			mockSession.Setup(c => c.TryGetValue(It.IsAny<string>(), out data!));
			/**
			 * cria um mock para o contexto, de forma que permite simular os dados que estão
			 * trafegando na requisição HTTP no processo de teste da página de carregar o carrinho
			 */
			Mock<HttpContext> mockContext = new();
			mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

			// Action
			CartModel cartModel = new(mockRepo.Object)
			{
				PageContext = new PageContext(new ActionContext
				{
					HttpContext = mockContext.Object,
					RouteData = new RouteData(),
					ActionDescriptor = new PageActionDescriptor()
				})
			};
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
				new() { ProductID = 1, Name = "P1" }
			}).AsQueryable<Product>());

			Cart? testCart = new();

			Mock<ISession> mockSession = new();
			mockSession
				.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
				.Callback<string, byte[]>((key, val) => testCart = JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val)));

			Mock<HttpContext> mockContext = new();
			mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

			// Action
			CartModel cartModel = new(mockRepo.Object)
			{
				PageContext = new PageContext(new ActionContext
				{
					HttpContext = mockContext.Object,
					RouteData = new RouteData(),
					ActionDescriptor = new PageActionDescriptor()
				})
			};
			cartModel.OnPost(1, "myUrl");

			//Assert
			Assert.Single(testCart.Lines);
			Assert.Equal("P1", testCart.Lines.First().Product.Name);
			Assert.Equal(1, testCart.Lines.First().Quantity);
		}
	}
}