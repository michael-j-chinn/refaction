using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace refactor_me.Views
{
	public class ProductOptionDto
	{
		[Required]
		[MaxLength(1000)]
		public string Name { get; set; }

		[MaxLength(8000)]
		public string Description { get; set; }
	}
}