using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComputerBeacon.Json;

namespace ExHentaiAPI
{
	public class GalleryInfo
	{
		internal GalleryInfo(JsonObject jo)
		{
			int i;
			float f;

			this.token = new GalleryToken((int)jo["gid"], (string)jo["token"]);

			this.error = (string)jo["error"];
			this.ArchiverKey = (string)jo["archiver_key"];
			this.ArchiverKeyTime = DateTime.Now.AddHours(1);
			this.ArchiverKeyTimeUTC = DateTime.UtcNow.AddHours(1);
			this.Title = (string)jo["title"];
			this.TitleJpn = (string)jo["title_jpn"];
			this.ThumbURL = (string)jo["thumb"];
			this.Uploader = (string)jo["uploader"];
			this.Expunged = (bool)jo["expunged"];
			this.FileSize = (int)jo["filesize"];

			if (int.TryParse((string)jo["torrentcount"], out i))
				this.TorrentCount = i;
			else
				this.TorrentCount = 0;

			if (int.TryParse((string)jo["posted"], out i))
			{
				this.PostedUTC = Helper.ToDateTime(i);
				this.Posted = this.PostedUTC.ToLocalTime();
			}

			if (int.TryParse((string)jo["filecount"], out i))
				this.FileCount = i;
			else
				this.FileCount = 0;

			if (float.TryParse((string)jo["rating"], out f))
				this.Rating = f;
			else
				this.Rating = 0;

			this.Category = Helper.ToCategories((string)jo["category"]);

			if (jo.ContainsKey("tags"))
			{
				JsonArray ja = (JsonArray)jo["tags"];
				this.Tags = new string[ja.Count];
				for (i = 0; i < ja.Count; i++)
					this.Tags[i] = (string)ja[i];
			}
		}

		public override string ToString()
		{
			return this.Title;
		}

		public GalleryToken token { get; internal set; }
		public string ArchiverKey { get; internal set; }
		public DateTime ArchiverKeyTime { get; internal set; }
		public DateTime ArchiverKeyTimeUTC { get; internal set; }
		public string Title { get; internal set; }
		public string TitleJpn { get; internal set; }
		public Categories Category { get; internal set; }
		public string ThumbURL { get; internal set; }
		public string ThumbPath { get; set; }
		public string Uploader { get; internal set; }
		public DateTime PostedUTC { get; internal set; }
		public DateTime Posted { get; internal set; }
		public int FileCount { get; internal set; }
		public int FileSize { get; internal set; }
		public bool Expunged { get; internal set; }
		public float Rating { get; internal set; }
		public int TorrentCount { get; internal set; }
		public string error { get; internal set; }
		public string[] Tags { get; internal set; }
	}
}
