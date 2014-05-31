using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ComputerBeacon.Json;

namespace ExHentaiAPI
{
	public class TorrentInfoCollection : List<TorrentInfo>
	{
		public TorrentInfoCollection()
			: base()
		{
		}
		public TorrentInfoCollection(IEnumerable<TorrentInfo> collection)
			: base(collection)
		{
		}

		private static Regex rCount		= new Regex("Showing ([0-9,]+)-([0-9,]+) of ([0-9,]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static Regex rSearch	= new Regex("<tr class=\"gtr", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static Regex rForm	= new Regex("<form method=\"post\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		internal TorrentInfoCollection(string body)
		{
			Match m = rCount.Match(body);
			if (m.Success)
			{
				try
				{
					this.ShowingStart	= int.Parse(m.Groups[1].Value);
					this.ShowingEnd		= int.Parse(m.Groups[2].Value);
					this.SearchCount	= int.Parse(m.Groups[3].Value.Replace(",", ""));
				}
				catch
				{
					this.ShowingStart = this.ShowingEnd = this.SearchCount = 0;
				}
			}

			string[] arr = rSearch.Split(body);
			for (int i = 0; i < arr.Length; ++i)
			{
				try
				{
					this.Add(new TorrentInfo(arr[i]));
				}
				catch
				{
				}
			}
		}


		internal TorrentInfoCollection(string body, GalleryToken token)
		{
			this.ShowingStart = this.ShowingEnd = this.SearchCount = 0;

			string[] arr = rForm.Split(body);
			for (int i = 0; i < arr.Length; ++i)
			{
				try
				{
					this.Add(new TorrentInfo(arr[i], token));
				}
				catch
				{
				}
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public int ShowingStart	{ get; internal set; }
		public int ShowingEnd { get; internal set; }
		public int SearchCount { get; internal set; }
	}
}
