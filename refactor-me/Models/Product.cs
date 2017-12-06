using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
	public class Product
	{
		public Guid Id { get; set; }

		[Required]
		[MaxLength(1000)]
		public string Name { get; set; }

		[MaxLength(8000)]
		public string Description { get; set; }

		[Required]
		[Range(0, Double.MaxValue)]
		public decimal Price { get; set; }

		[Required]
		[Range(0, Double.MaxValue)]
		public decimal DeliveryPrice { get; set; }

		public virtual ICollection<ProductOption> Options { get; set; }
	}
}