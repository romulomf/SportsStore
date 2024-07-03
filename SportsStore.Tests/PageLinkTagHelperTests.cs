using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SportsStore.Tests
{
	public class PageLinkTagHelperTests
	{
		[Fact]
		public void Can_Generate_Page_Links()
		{
			// Arrange
			var urlHelper = new Mock<IUrlHelper>();
			urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
				.Returns("Test/Page1")
				.Returns("Test/Page2")
				.Returns("Test/Page3");

			var urlHelperFactory = new Mock<IUrlHelperFactory>();
			urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

			var viewContext = new Mock<ViewContext>();

			PageLinkTagHelper helper = new(urlHelperFactory.Object)
			{
				PageModel = new PagingInfo
				{
					TotalItems = 28,
					ItemsPerPage = 10,
					CurrentPage = 2,
				},
				ViewContext = viewContext.Object,
				PageAction = "Test"
			};

			TagHelperContext ctx = new([], new Dictionary<object, object>(), "");

			var content = new Mock<TagHelperContent>();
			TagHelperOutput output = new("div", [], (cache, encoder) => Task.FromResult(content.Object));

			// Act
			helper.Process(ctx, output);

			// Assert
			Assert.Equal(@"<a href=""Test/Page1"">1</a>" + @"<a href=""Test/Page2"">2</a>" + @"<a href=""Test/Page3"">3</a>", output.Content.GetContent());
		}
	}
}