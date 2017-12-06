using refactor_me.Models;
using refactor_me.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
		public IHttpActionResult GetAllOptions(Guid productId)
		{
			return Ok(_productOptionsRepository.GetAll(productId));
		}

		[Route("{id}")]
		[HttpGet]
		public IHttpActionResult GetOption(Guid productId, Guid id)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			var productOption = _productOptionsRepository.GetById(id, productId);

			if (productOption == null)
				return NotFound();
			else
				return Ok(productOption);
		}

		[Route]
		[HttpPost]
		public IHttpActionResult CreateOption(Guid productId, [FromBody] ProductOption productOption)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var productToAddOptionTo = _productRepository.GetById(productId);

			if (productToAddOptionTo == null)
				return BadRequest("Product doesn't exist.");

			_productOptionsRepository.Save(productId, productOption);

			var newProductOption = _productRepository.GetById(productOption.Id);

			if (newProductOption == null)
				return InternalServerError();
			else
				return Created("TODO: URI", newProductOption);
		}

		[Route("{id}")]
		[HttpPut]
		public IHttpActionResult UpdateOption(Guid productId, Guid id, [FromBody] ProductOption updatedProductOption)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var existingProduct = _productRepository.GetById(id);

			if (existingProduct == null)
				return BadRequest("Product doesn't exist.");

			_productOptionsRepository.Update(id, productId, updatedProductOption);

			return Ok();
		}

		[Route("{productId}/options/{id}")]
		[HttpDelete]
		public IHttpActionResult DeleteOption(Guid productId, Guid id)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			var existingProduct = _productRepository.GetById(id);

			if (existingProduct == null)
				return BadRequest("Product doesn't exist.");

			var productOptionToDelete = _productOptionsRepository.GetById(id, productId);

			if (productOptionToDelete == null)
				return NotFound();

			_productOptionsRepository.Delete(id, productId);

			return Ok();
		}
	}
}
