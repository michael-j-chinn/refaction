using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.Views
{
	public class Products
	{
		public List<Product> Items { get; set; }
		public Paging Paging { get; set; }
	}
}