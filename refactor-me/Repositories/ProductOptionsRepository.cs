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
		Task<ProductOptions> GetAllAsync(Guid productId);
		Task<ProductOption> GetByIdAsync(Guid id, Guid productId);
		Task SaveAsync(Guid productId, ProductOption productOption);
		Task UpdateAsync(Guid Id, Guid productId, ProductOption productOption);
		Task DeleteAsync(Guid Id, Guid productId);
	}

	public class ProductOptionsRepository : IProductOptionsRepository
	{
		public async Task<ProductOptions> GetAllAsync(Guid productId)
		{
			var productOptions = new ProductOptions();

			using (var context = new ProductContext())
			{
				productOptions.Items = await context.ProductOptions
					.Where(p => p.ProductId == productId)
					.ToListAsync();
			}

			return productOptions;
		}

		public async Task<ProductOption> GetByIdAsync(Guid id, Guid productId)
		{
			ProductOption productOption = null;

			using (var context = new ProductContext())
			{
				productOption = await context.ProductOptions
					.Where(p => p.Id == id && p.ProductId == productId)
					.FirstOrDefaultAsync();
			}

			return productOption;
		}

		public async Task SaveAsync(Guid productId, ProductOption productOption)
		{
			using (var context = new ProductContext())
			{
				var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

				if (product != null)
				{
					product.Options.Add(productOption);
					await context.SaveChangesAsync();
				}
			}
		}

		public async Task UpdateAsync(Guid Id, Guid productId, ProductOption updatedProductOption)
		{
			using (var context = new ProductContext())
			{
				var existingProductOption = await context.ProductOptions
					.FirstOrDefaultAsync(p => p.Id == Id && p.ProductId == productId);

				context.Entry(existingProductOption).CurrentValues.SetValues(updatedProductOption);

				await context.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Guid Id, Guid productId)
		{
			using (var context = new ProductContext())
			{
				var existingProductOption = await context.ProductOptions
					.FirstOrDefaultAsync(p => p.Id == Id && p.ProductId == productId);

				if (existingProductOption != null)
				{
					context.ProductOptions.Remove(existingProductOption);
					await context.SaveChangesAsync();
				}
			}
		}
	}
}