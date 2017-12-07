using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Controllers;
using Moq;
using refactor_me.Repositories;
using System.Threading.Tasks;
using System.Web.Http.Results;
using refactor_me.Views;
using System.Collections.Generic;
using refactor_me.Models;
using System.Net.Http;
using System.Web.Http.Routing;

namespace refactor_me_tests
{
	[TestClass]
	public class UnitTestProductsController
	{
		[TestMethod]
		public async Task TestProductsGetAllAsync_ShouldBeOneResultAndOk()
		{
			var url = new Uri("http://localhost:8080/api/v1/products");
			var mockProductRepo = new Mock<IProductRepository>();

			mockProductRepo
				.Setup(p => p.GetAllAsync(url, 1, 0))
				.ReturnsAsync(new Products
				{
					Items = new List<Product>
					{
						new Product { Name = "First" }
					}
				});

			var mockLoggerService = new Mock<ILoggerService>();

			var controller = new ProductsController(mockProductRepo.Object, mockLoggerService.Object);
			controller.Url = new UrlHelper(new HttpRequestMessage(HttpMethod.Get, url));

			var result = await controller.GetAllAsync(1, 0) as OkNegotiatedContentResult<Products>;

			Assert.IsNotNull(result);
			Assert.IsTrue(result.Content.Items.Count == 1);
		}
	}
}
