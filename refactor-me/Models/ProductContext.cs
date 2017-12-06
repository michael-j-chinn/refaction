using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
	public class ProductContext : DbContext
	{
		public ProductContext() : base("ProductsContext")
		{
			Database.SetInitializer<ProductContext>(new ProductInitializer());
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<ProductOption> ProductOptions { get; set; }
	}
}