﻿using System;
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
		private ILoggerService _logger;

		public ProductsController(IProductRepository productRepository, ILoggerService logger)
		{
			_productRepository = productRepository;
			_logger = logger;
		}

		[Route]
		[HttpGet]
		public async Task<IHttpActionResult> GetAllAsync(int limit = 1, int offset = 0)
		{
			return Ok(await _productRepository.GetAllAsync(Url.Request.RequestUri, limit, offset));
		}

		[Route("search")]
		[HttpGet]
		public async Task<IHttpActionResult> SearchAsync(string name, int limit = 1, int offset = 0)
		{
			return Ok(await _productRepository.GetByNameAsync(Url.Request.RequestUri, name, limit, offset));
		}

		[Route("{id}")]
		[HttpGet]
		public async Task<IHttpActionResult> GetProductAsync(Guid id)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			// Verify id is valid.
			var product = await _productRepository.GetByIdAsync(id);

			if (product == null)
				return NotFound();
			else
				return Ok(product);
		}

		[HttpPost]
		public async Task<IHttpActionResult> CreateAsync([FromBody] ProductDto product)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Map Dto to Model
			var productToCreate = new Product();
			productToCreate.MapDto(product);

			await _productRepository.SaveAsync(productToCreate);

			// Verify save was successful
			var newProduct = await _productRepository.GetByIdAsync(productToCreate.Id);

			if (newProduct == null)
				return InternalServerError();
			else
				return Created(new EntityUrlByID(Url.Request.RequestUri, newProduct.Id).ToString(), newProduct);
		}

		[Route("{id}")]
		[HttpPut]
		public async Task<IHttpActionResult> UpdateAsync(Guid id, [FromBody] ProductDto updatedProduct)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Verify id is valid.
			var existingProduct = await _productRepository.GetByIdAsync(id);

			if (existingProduct == null)
				return NotFound();

			// Map Dto to Model
			var productToUpdate = new Product();
			productToUpdate.MapDto(updatedProduct);

			await _productRepository.UpdateAsync(id, productToUpdate);

			return Ok();
		}

		[Route("{id}")]
		[HttpDelete]
		public async Task<IHttpActionResult> DeleteAsync(Guid id)
		{
			if (id == null)
				return BadRequest("An ID must be provided.");

			// Verify id is valid.
			var productToDelete = await _productRepository.GetByIdAsync(id);

			if (productToDelete == null)
				return NotFound();

			await _productRepository.DeleteAsync(id);

			return Ok();
		}
	}
}
