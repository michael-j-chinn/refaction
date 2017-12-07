using refactor_me.Models;
using refactor_me.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace refactor_me.Controllers.v1
{
	[RoutePrefix("api/v1/products/{productId}/options")]
	public class ProductOptionsController : ApiController
	{
		private IProductRepository _productRepository;
		private IProductOptionsRepository _productOptionsRepository;

		public ProductOptionsController(IProductRepository productRepository, IProductOptionsRepository productOptionsRepository)
		{
			_productRepository = productRepository;
			_productOptionsRepository = productOptionsRepository;
		}

		[Route]
		[HttpGet]
		public async Task<IHttpActionResult> GetAllOptionsAsync(Guid productId, int limit = 1, int offset = 0)
		{
			return Ok(await _productOptionsRepository.GetAllAsync(productId, Url.Request.RequestUri, limit, offset));
		}

		[Route("{id}")]
		[HttpGet]
		public async Task<IHttpActionResult> GetOptionAsync(Guid productId, Guid id)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			var productOption = await _productOptionsRepository.GetByIdAsync(id, productId);

			if (productOption == null)
				return NotFound();
			else
				return Ok(productOption);
		}

		[Route]
		[HttpPost]
		public async Task<IHttpActionResult> CreateOptionAsync(Guid productId, [FromBody] ProductOption productOption)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var productToAddOptionTo = await _productRepository.GetByIdAsync(productId);

			if (productToAddOptionTo == null)
				return BadRequest("Product doesn't exist.");

			await _productOptionsRepository.SaveAsync(productId, productOption);

			var newProductOption = await _productRepository.GetByIdAsync(productOption.Id);

			if (newProductOption == null)
				return InternalServerError();
			else
				return Created("TODO: URI", newProductOption);
		}

		[Route("{id}")]
		[HttpPut]
		public async Task<IHttpActionResult> UpdateOptionAsync(Guid productId, Guid id, [FromBody] ProductOption updatedProductOption)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var existingProduct = await _productRepository.GetByIdAsync(id);

			if (existingProduct == null)
				return BadRequest("Product doesn't exist.");

			await _productOptionsRepository.UpdateAsync(id, productId, updatedProductOption);

			return Ok();
		}

		[Route("{id}")]
		[HttpDelete]
		public async Task<IHttpActionResult> DeleteOptionAsync(Guid productId, Guid id)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			var existingProduct = await _productRepository.GetByIdAsync(id);

			if (existingProduct == null)
				return BadRequest("Product doesn't exist.");

			var productOptionToDelete = await _productOptionsRepository.GetByIdAsync(id, productId);

			if (productOptionToDelete == null)
				return NotFound();

			await _productOptionsRepository.DeleteAsync(id, productId);

			return Ok();
		}
	}
}
