using refactor_me.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
	public class ProductOption
	{
		private Guid id = Guid.Empty;

		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id
		{
			get
			{
				if (id == Guid.Empty)
					id = Guid.NewGuid();
				return id;
			}
			set { id = value; }
		}

		public Guid ProductId { get; set; }

		[Required]
		[MaxLength(1000)]
		public string Name { get; set; }

		[MaxLength(8000)]
		public string Description { get; set; }

		public void MapDto(ProductOptionDto dto, Guid productId)
		{
			ProductId = productId;
			Name = dto.Name;
			Description = dto.Description;
		}
	}
}