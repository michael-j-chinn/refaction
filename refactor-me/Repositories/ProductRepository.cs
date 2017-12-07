using refactor_me.Models;
using refactor_me.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace refactor_me.Repositories
{
	public interface IProductRepository
	{
		Task<Products> GetAllAsync(Uri url, int limit, int offset);
		Task<Products> GetByNameAsync(Uri url, string name, int limit, int offset);
		Task<Product> GetByIdAsync(Guid id);
		Task SaveAsync(Product product);
		Task UpdateAsync(Guid id, Product product);
		Task DeleteAsync(Guid id);
	}

	public class ProductRepository : IProductRepository
	{
		private ILoggerService _logger;

		public ProductRepository(ILoggerService logger)
		{
			_logger = logger;
		}

		public async Task<Products> GetAllAsync(Uri url, int limit = 1, int offset = 0)
		{
			var products = new Products();

			try
			{
				using (var context = new ProductContext())
				{
					var query = context.Products;
					var totalRecords = await query.CountAsync();

					products.Paging = new Paging(url, totalRecords, limit, offset);

					products.Items = await query
						.OrderBy(p => p.Name)
						.Skip(products.Paging.AdjustedOffset)
						.Take(limit)
						.Include(p => p.Options)
						.ToListAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while retrieving all products.", ex, new Dictionary<string, object> { { "requestUrl", url.AbsoluteUri } });
			}

			return products;
		}

		public async Task<Products> GetByNameAsync(Uri url, string name, int limit = 1, int offset = 0)
		{
			var products = new Products();

			try
			{
				using (var context = new ProductContext())
				{
					var query = context.Products.Where(p => p.Name.Contains(name));
					var totalRecords = await query.CountAsync();

					products.Paging = new Paging(url, totalRecords, limit, offset);

					products.Items = await context.Products
						.OrderBy(p => p.Name)
						.Skip(products.Paging.AdjustedOffset)
						.Take(limit)
						.Include(p => p.Options)
						.ToListAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while searching products by name.", ex, new Dictionary<string, object> { { "requestUrl", url.AbsoluteUri }, { "name", name } });
			}

			return products;
		}

		public async Task<Product> GetByIdAsync(Guid id)
		{
			Product product = null;

			try
			{
				using (var context = new ProductContext())
				{
					product = await context.Products
									 .Where(p => p.Id == id)
									 .FirstOrDefaultAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while searching products by ID.", ex, new Dictionary<string, object> { { "id", id } });
			}

			return product;
		}

		public async Task SaveAsync(Product product)
		{
			try
			{
				using (var context = new ProductContext())
				{
					context.Products.Add(product);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while saving new product.", ex, new Dictionary<string, object> { { "product", JsonConvert.SerializeObject(product) } });
			}
		}

		public async Task UpdateAsync(Guid id, Product updatedProduct)
		{
			try
			{
				using (var context = new ProductContext())
				{
					var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

					context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);

					await context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while updating product.", ex, new Dictionary<string, object> { { "id", id }, { "updatedProduct", JsonConvert.SerializeObject(updatedProduct) } });
			}
		}

		public async Task DeleteAsync(Guid id)
		{
			try
			{
				using (var context = new ProductContext())
				{
					var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

					if (existingProduct != null)
					{
						context.Products.Remove(existingProduct);
						await context.SaveChangesAsync();
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while deleting product.", ex, new Dictionary<string, object> { { "id", id } });
			}
		}
	}
}