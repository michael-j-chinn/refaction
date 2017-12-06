using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
	public class ProductOption
	{
		public Guid Id { get; set; }

		public Guid ProductId { get; set; }

		[Required]
		[MaxLength(1000)]
		public string Name { get; set; }

		[MaxLength(8000)]
		public string Description { get; set; }
	}
}