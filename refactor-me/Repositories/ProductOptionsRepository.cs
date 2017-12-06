using refactor_me.Models;
using refactor_me.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.Repositories
{
	public interface IProductOptionsRepository
	{
		ProductOptions GetAll(Guid productId);
		ProductOption GetById(Guid id, Guid productId);
		void Save(Guid productId, ProductOption productOption);
		void Update(Guid Id, Guid productId, ProductOption productOption);
		void Delete(Guid Id, Guid productId);
	}

	public class ProductOptionsRepository : IProductOptionsRepository
	{
		public ProductOptions GetAll(Guid productId)
		{
			var productOptions = new ProductOptions();

			using (var context = new ProductContext())
			{
				productOptions.Items = context.ProductOptions.Where(p => p.ProductId == productId).ToList();
			}

			return productOptions;
		}

		public ProductOption GetById(Guid id, Guid productId)
		{
			ProductOption productOption = null;

			using (var context = new ProductContext())
			{
				productOption = context.ProductOptions
								 .Where(p => p.Id == id && p.ProductId == productId)
								 .FirstOrDefault();
			}

			return productOption;
		}

		public void Save(Guid productId, ProductOption productOption)
		{
			using (var context = new ProductContext())
			{
				var product = context.Products.FirstOrDefault(p => p.Id == productId);

				if (product != null)
				{
					product.Options.Add(productOption);
					context.SaveChanges();
				}
			}
		}

		public void Update(Guid Id, Guid productId, ProductOption updatedProductOption)
		{
			using (var context = new ProductContext())
			{
				var existingProductOption = context.ProductOptions.FirstOrDefault(p => p.Id == Id && p.ProductId == productId);

				context.Entry(existingProductOption).CurrentValues.SetValues(updatedProductOption);

				context.SaveChanges();
			}
		}

		public void Delete(Guid Id, Guid productId)
		{
			using (var context = new ProductContext())
			{
				var existingProductOption = context.ProductOptions.FirstOrDefault(p => p.Id == Id && p.ProductId == productId);

				if (existingProductOption != null)
				{
					context.ProductOptions.Remove(existingProductOption);
					context.SaveChanges();
				}
			}
		}
	}
}