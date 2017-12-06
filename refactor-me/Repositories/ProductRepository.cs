using refactor_me.Models;
using refactor_me.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace refactor_me.Repositories
{
	public interface IProductRepository
	{
		Task<Products> GetAllAsync();
		Task<Products> GetByNameAsync(string name);
		Task<Product> GetByIdAsync(Guid Id);
		Task SaveAsync(Product product);
		Task UpdateAsync(Guid Id, Product product);
		Task DeleteAsync(Guid Id);
	}

	public class ProductRepository : IProductRepository
	{
		public async Task<Products> GetAllAsync()
		{
			var products = new Products();

			using (var context = new ProductContext())
			{
				products.Items = await context.Products
										.Include(p => p.Options)
										.ToListAsync();
			}

			return products;
		}

		public async Task<Products> GetByNameAsync(string name)
		{
			var products = new Products();

			using (var context = new ProductContext())
			{
				products.Items = await context.Products
										.Include(p => p.Options)
										.Where(p => p.Name.Contains(name))
										.ToListAsync();
			}

			return products;
		}

		public async Task<Product> GetByIdAsync(Guid Id)
		{
			Product product = null;

			using (var context = new ProductContext())
			{
				product = await context.Products
								 .Where(p => p.Id == Id)
								 .FirstOrDefaultAsync();
			}

			return product;
		}

		public async Task SaveAsync(Product product)
		{
			using (var context = new ProductContext())
			{
				context.Products.Add(product);
				await context.SaveChangesAsync();
			}
		}

		public async Task UpdateAsync(Guid Id, Product updatedProduct)
		{
			using (var context = new ProductContext())
			{
				var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == Id);

				context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);

				await context.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Guid Id)
		{
			using (var context = new ProductContext())
			{
				var existing_product = await context.Products.FirstOrDefaultAsync(p => p.Id == Id);

				if (existing_product != null)
				{
					context.Products.Remove(existing_product);
					await context.SaveChangesAsync();
				}
			}
		}
	}
}