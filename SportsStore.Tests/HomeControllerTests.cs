using Xunit;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests
{
	public class HomeControllerTests
	{
		[Fact]
		public void CanUseRepository()
		{
			// Arrange
			Mock<IStoreRepository> mock = new();
			mock.Setup(m => m.Products).Returns((new Product[] {
				new() {ProductID = 1, Name = "P1"},
				new() {ProductID = 2, Name = "P2"}
			}).AsQueryable<Product>());

			HomeController controller = new(mock.Object);

			// Act
			ProductsListViewModel result = controller.Index(null)?.ViewData.Model as ProductsListViewModel ?? new();

			// Assert
			Product[] prodArray = result.Products.ToArray();
			Assert.True(prodArray.Length == 2);
			Assert.Equal("P1", prodArray[0].Name);
			Assert.Equal("P2", prodArray[1].Name);
		}

		[Fact]
		public void Can_Send_Pagination_View_Model()
		{
			// Arrange
			Mock<IStoreRepository> mock = new();
			mock.Setup(m => m.Products).Returns((new Product[] {
				new() {ProductID = 1, Name = "P1"},
				new() {ProductID = 2, Name = "P2"},
				new() {ProductID = 3, Name = "P3"},
				new() {ProductID = 4, Name = "P4"},
				new() {ProductID = 5, Name = "P5"}
			}).AsQueryable<Product>());

			HomeController controller = new(mock.Object)
			{
				PageSize = 3
			};

			// Act
			ProductsListViewModel result = controller.Index(null, 2)?.ViewData.Model as ProductsListViewModel ?? new();

			// Assert
			PagingInfo pageInfo = result.PagingInfo;
			Assert.Equal(2, pageInfo.CurrentPage);
			Assert.Equal(3, pageInfo.ItemsPerPage);
			Assert.Equal(5, pageInfo.TotalItems);
			Assert.Equal(2, pageInfo.TotalPages);
		}

		[Fact]
		public void Can_Filter_Products()
		{
			// Arrange
			// - create the mock repository
			Mock<IStoreRepository> mock = new();
			mock.Setup(m => m.Products).Returns((new Product[] {
				new() { ProductID = 1, Name = "P1", Category = "Cat1" },
				new() { ProductID = 2, Name = "P2", Category = "Cat2" },
				new() { ProductID = 3, Name = "P3", Category = "Cat1" },
				new() { ProductID = 4, Name = "P4", Category = "Cat2" },
				new() { ProductID = 5, Name = "P5", Category = "Cat3" }
			}).AsQueryable<Product>());

			// Arrange - create a controller and make the page size 3 items
			HomeController controller = new(mock.Object)
			{
				PageSize = 3
			};

			// Action
			Product[] result = (controller.Index("Cat2", 1)?.ViewData.Model as ProductsListViewModel ?? new()).Products.ToArray();

			// Assert
			Assert.Equal(2, result.Length);
			Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
			Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
		}
	}
}