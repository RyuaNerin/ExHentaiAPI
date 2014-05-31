using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ComputerBeacon.Json;

namespace ExHentaiAPI
{
	public class GalleryTokenCollection : List<GalleryToken>
	{
		public GalleryTokenCollection()
			: base()
		{
		}
		public GalleryTokenCollection(IEnumerable<GalleryToken> collection)
			: base(collection)
		{
		}

		private static Regex rCount		= new Regex("Showing ([0-9,]+)-([0-9,]+) of ([0-9,]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static Regex rGallery	= new Regex("http://[^/\"]+/g/([^/\"]+)/([^/\"]+)/", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		internal GalleryTokenCollection(string body)
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

			m = rGallery.Match(body);
			while (m.Success)
			{
				try
				{
					GalleryToken gt = new GalleryToken(int.Parse(m.Groups[1].Value), m.Groups[2].Value);
					if (!this.Contains(gt))
						this.Add(gt);
				}
				catch { }

				m = m.NextMatch();
			}
		}

		public int ShowingStart	{ get; internal set; }
		public int ShowingEnd { get; internal set; }
		public int SearchCount { get; internal set; }

		internal byte[] ToJsonArray()
		{
			return this.ToJsonArray(0, this.Count);
		}
		internal byte[] ToJsonArray(int offset, int length)
		{
			JsonObject jo = new JsonObject();
			jo.Add("method", "gdata");

			JsonArray jag = new JsonArray();
			JsonArray jao = new JsonArray();

			for (int i = offset; (i < offset + length) && (i < this.Count); i++)
			{
				jag.Add(new JsonArray { this[i].gid, this[i].token });
			}
			jo.Add("gidlist", jag);

			return Encoding.ASCII.GetBytes(jo.ToString());
		}
	}
}
