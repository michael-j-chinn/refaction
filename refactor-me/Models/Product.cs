using Newtonsoft.Json;
using refactor_me.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace refactor_me.Models
{
	public class Product
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

		public void MapDto(ProductDto dto)
		{
			Name = dto.Name;
			Description = dto.Description;
			Price = dto.Price;
			DeliveryPrice = dto.DeliveryPrice;
		}

		public virtual ICollection<ProductOption> Options { get; set; }
	}
}