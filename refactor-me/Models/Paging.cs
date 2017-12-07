using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace refactor_me.Models
{
	public class Paging
	{
		public string Next { get; private set; }
		public string Prev { get; private set; }

		[JsonIgnore]
		public int AdjustedOffset { get; private set; }

		public Paging(Uri url, int totalRecords, int limit, int offset)
		{
			if (totalRecords > 0)
			{
				AdjustedOffset = offset > totalRecords ? totalRecords : offset;

				// Extract just the base endpoint url
				var base_url = $"{url.Scheme}://{url.Authority}{url.AbsolutePath}";

				// Extract any query string params other than limit and offset
				var nonPagingParams = new NonPagingQueryParamsList(url.Query);

				// Calculate how many records are left
				var remainingRecords = totalRecords - offset;
				var nextOffset = offset + Math.Min(limit, remainingRecords);

				// Construct the next page url
				if (nextOffset < totalRecords)
				{
					Next = $"{base_url}?limit={limit}&offset={nextOffset}{(nonPagingParams.Count > 0 ? "&" + nonPagingParams.ToString() : "")}";
				}

				// Construct the previous page url
				if (offset > 0)
				{
					var prevOffset = Math.Max(offset - limit, 0);
					Prev = $"{base_url}?limit={limit}&offset={prevOffset}{(nonPagingParams.Count > 0 ? "&" + nonPagingParams.ToString() : "")}";
				}
			}
		}
	}
}