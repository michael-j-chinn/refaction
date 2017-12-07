using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
	public class NonPagingQueryParamsList : List<string>
	{
		private string[] pagingParams = new string[] { "limit", "offset" };

		public NonPagingQueryParamsList(string queryParams)
		{
			if (!string.IsNullOrWhiteSpace(queryParams))
			{
				// Remove the questions mark if it's present
				if (queryParams[0] == '?')
					queryParams = queryParams.Substring(1);

				// Break the query string down into individual parts
				var parts = queryParams.Split('&');

				// Filter out the paging related params
				this.AddRange(parts.Where(p => !pagingParams.Contains(p.Split('=')[0])));
			}
		}

		public override string ToString()
		{
			return string.Join("&", this);
		}
	}
}