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
		Products GetAll();
		Products GetByName(string name);
		Product GetById(Guid Id);
		void Save(Product product);
		void Update(Guid Id, Product product);
		void Delete(Guid Id);
	}

	public class ProductRepository : IProductRepository
	{
		public Products GetAll()
		{
			var products = new Products();

			using (var context = new ProductContext())
			{
				products.Items = context.Products
										.Include(p => p.Options)
										.ToList();
			}

			return products;
		}

		public Products GetByName(string name)
		{
			var products = new Products();

			using (var context = new ProductContext())
			{
				products.Items = context.Products
										.Include(p => p.Options)
										.Where(p => p.Name.Contains(name))
										.ToList();
			}

			return products;
		}

		public Product GetById(Guid Id)
		{
			Product product = null;

			using (var context = new ProductContext())
			{
				product = context.Products
								 .Where(p => p.Id == Id)
								 .FirstOrDefault();
			}

			return product;
		}

		public void Save(Product product)
		{
			using (var context = new ProductContext())
			{
				context.Products.Add(product);
				context.SaveChanges();
			}
		}

		public void Update(Guid Id, Product updatedProduct)
		{
			using (var context = new ProductContext())
			{
				var existingProduct = context.Products.FirstOrDefault(p => p.Id == Id);

				context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);

				context.SaveChanges();
			}
		}

		public void Delete(Guid Id)
		{
			using (var context = new ProductContext())
			{
				var existing_product = context.Products.FirstOrDefault(p => p.Id == Id);

				if (existing_product != null)
				{
					context.Products.Remove(existing_product);
					context.SaveChanges();
				}
			}
		}
	}
}