using Newtonsoft.Json;
using refactor_me.Models;
using refactor_me.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace refactor_me.Repositories
{
	public interface IProductOptionsRepository
	{
		Task<ProductOptions> GetAllAsync(Guid productId, Uri url, int limit, int offset);
		Task<ProductOption> GetByIdAsync(Guid id, Guid productId);
		Task SaveAsync(Guid productId, ProductOption productOption);
		Task UpdateAsync(Guid id, Guid productId, ProductOption productOption);
		Task DeleteAsync(Guid id, Guid productId);
	}

	public class ProductOptionsRepository : IProductOptionsRepository
	{
		private ILoggerService _logger;

		public ProductOptionsRepository(ILoggerService logger)
		{
			_logger = logger;
		}

		public async Task<ProductOptions> GetAllAsync(Guid productId, Uri url, int limit, int offset)
		{
			var productOptions = new ProductOptions();

			try
			{
				using (var context = new ProductContext())
				{
					var query = context.ProductOptions.Where(p => p.ProductId == productId);
					var totalRecords = await query.CountAsync();

					productOptions.Paging = new Paging(url, totalRecords, limit, offset);

					productOptions.Items = await query
						.OrderBy(p => p.Name)
						.Skip(productOptions.Paging.AdjustedOffset)
						.Take(limit)
						.ToListAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while retrieving all product options.", ex, new Dictionary<string, object> { { "requestUrl", url.AbsoluteUri } });
			}

			return productOptions;
		}

		public async Task<ProductOption> GetByIdAsync(Guid id, Guid productId)
		{
			ProductOption productOption = null;

			try
			{
				using (var context = new ProductContext())
				{
					productOption = await context.ProductOptions
						.Where(p => p.Id == id && p.ProductId == productId)
						.FirstOrDefaultAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while retrieving all product option by ID.", ex, new Dictionary<string, object> { { "id", id }, { "productId", productId } });
			}

			return productOption;
		}

		public async Task SaveAsync(Guid productId, ProductOption productOption)
		{
			try
			{
				using (var context = new ProductContext())
				{
					context.ProductOptions.Add(productOption);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while saving product option.", ex, new Dictionary<string, object> { { "productId", productId }, { "productOption", JsonConvert.SerializeObject(productOption) } });
			}
		}

		public async Task UpdateAsync(Guid id, Guid productId, ProductOption updatedProductOption)
		{
			try
			{
				using (var context = new ProductContext())
				{
					var existingProductOption = await context.ProductOptions
						.FirstOrDefaultAsync(p => p.Id == id && p.ProductId == productId);

					existingProductOption.Name = updatedProductOption.Name;
					existingProductOption.Description = updatedProductOption.Description;

					await context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while updating product option.", ex, new Dictionary<string, object> { { "id", id }, { "productId", productId }, { "productOption", JsonConvert.SerializeObject(updatedProductOption) } });
			}
		}

		public async Task DeleteAsync(Guid id, Guid productId)
		{
			try
			{
				using (var context = new ProductContext())
				{
					var existingProductOption = await context.ProductOptions
						.FirstOrDefaultAsync(p => p.Id == id && p.ProductId == productId);

					if (existingProductOption != null)
					{
						context.ProductOptions.Remove(existingProductOption);
						await context.SaveChangesAsync();
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.ERROR, "Error while deleting product option.", ex, new Dictionary<string, object> { { "id", id }, { "productId", productId } });
			}
		}
	}
}