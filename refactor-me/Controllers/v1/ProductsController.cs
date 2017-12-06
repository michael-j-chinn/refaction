using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Views;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using refactor_me.Repositories;

namespace refactor_me.Controllers
{
	[RoutePrefix("api/v1/products")]
	public class ProductsController : ApiController
	{
		// Response code choices referenced from https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design
		private IProductRepository _productRepository;

		public ProductsController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		[Route]
		[HttpGet]
		public IHttpActionResult GetAll()
		{
			return Ok(_productRepository.GetAll());
		}

		[Route]
		[HttpGet]
		public IHttpActionResult SearchByName(string name)
		{
			return Ok(_productRepository.GetByName(name));
		}

		[Route("{id}")]
		[HttpGet]
		public IHttpActionResult GetProduct(Guid id)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			var product = _productRepository.GetById(id);

			if (product == null)
				return NotFound();
			else
				return Ok(product);
		}

		[HttpPost]
		public IHttpActionResult Create([FromBody] Product product)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			_productRepository.Save(product);

			var newProduct = _productRepository.GetById(product.Id);

			if (newProduct == null)
				return InternalServerError();
			else
				return Created("TODO: URI", newProduct);
		}

		[Route("{id}")]
		[HttpPut]
		public IHttpActionResult Update(Guid id, [FromBody] Product updatedProduct)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var existingProduct = _productRepository.GetById(id);

			if (existingProduct == null)
				return NotFound();

			_productRepository.Update(id, updatedProduct);

			return Ok();
		}

		[Route("{id}")]
		[HttpDelete]
		public IHttpActionResult Delete(Guid id)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			var productToDelete = _productRepository.GetById(id);

			if (productToDelete == null)
				return NotFound();

			_productRepository.Delete(id);

			return Ok();
		}
	}
}
