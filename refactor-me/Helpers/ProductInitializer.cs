using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace refactor_me.Helpers
{
	public class ProductInitializer : DropCreateDatabaseAlways<ProductContext>
	{
		protected override void Seed(ProductContext context)
		{
			base.Seed(context);

			var products = new List<Product>
			{
				new Product
				{
					Id = Guid.NewGuid(),
					Name = "Tampa Bay Buccaneers Hat",
					Description = "A beautiful hat from the 2002 world champion Tampa Bay Buccaneers",
					Price = 29.99m,
					DeliveryPrice = 32.78m,
					Options = new List<ProductOption>
					{
						new ProductOption
						{
							Id = Guid.NewGuid(),
							Name = "S",
							Description = "Size Small"
						},
						new ProductOption
						{
							Id = Guid.NewGuid(),
							Name = "L",
							Description = "Size Large"
						}
					}
				},
				new Product
				{
					Id = Guid.NewGuid(),
					Name = "Paperclip",
					Description = "Standard office paperclip",
					Price = 0.15m,
					DeliveryPrice = 0.16m
				},
				new Product
				{
					Id = Guid.NewGuid(),
					Name = "Cup",
					Description = "Cup with cat smiling",
					Price = 9.15m,
					DeliveryPrice = 10.16m
				},
				new Product
				{
					Id = Guid.NewGuid(),
					Name = "Cupboard",
					Description = "Cupboard to put your cat cups in",
					Price = 9.15m,
					DeliveryPrice = 10.16m
				}
			};

			context.Products.AddRange(products);
			context.SaveChanges();
		}
	}
}