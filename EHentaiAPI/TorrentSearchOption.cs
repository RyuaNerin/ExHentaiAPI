using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExHentaiAPI
{
	public class TorrentSearchOption
	{
		public enum OrderTypes { Added, Size, Seeds, Peers, Downloads }

		public TorrentSearchOption()
			: this(null, null)
		{
		}
		public TorrentSearchOption(string keyword)
			: this(keyword, null)
		{
		}
		public TorrentSearchOption(string keyword, int? page)
		{
			this.Keyword = keyword;
			this.Page = page;
		}

		public string Keyword { get; set; }
		public int? Page { get; set; }
		public OrderTypes? OrderType { get; set; }
		public bool Ascending { get; set; }

		internal string GetUrl()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("http://exhentai.org/torrents.php");

			if (String.IsNullOrEmpty(this.Keyword))
				sb.AppendFormat("?search={0}&", Uri.EscapeUriString(this.Keyword));

			if (this.Page.HasValue && this.Page.Value > 1)
				sb.AppendFormat("?page={0}", (this.Page - 1));

			if (this.OrderType != null)
			{
				sb.Append("?o=");

				switch (this.OrderType.Value)
				{
					case OrderTypes.Added:		sb.Append("a");	break;
					case OrderTypes.Size:		sb.Append("z"); break;
					case OrderTypes.Seeds:		sb.Append("s"); break;
					case OrderTypes.Peers:		sb.Append("d");	break;
					case OrderTypes.Downloads:	sb.Append("c");	break;
				}

				sb.Append(this.Ascending ? 'a' : 'd');
			}

			if (sb[sb.Length - 1] == '&')
				sb = sb.Remove(sb.Length - 1, 1);

			sb = sb.Replace("&?", "&");

			return sb.ToString();
		}
	}
}
