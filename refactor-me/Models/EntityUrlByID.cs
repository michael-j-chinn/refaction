using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
	public class EntityUrlByID
	{
		private string _url;

		public EntityUrlByID(Uri requestUrl, Guid id)
		{
			_url = $"{requestUrl.Scheme}://{requestUrl.Authority}{requestUrl.AbsolutePath}/{id}";
		}

		public override string ToString()
		{
			return _url;
		}
	}
}