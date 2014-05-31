using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExHentaiAPI
{
	public class TorrentInfo
	{
		private static Regex rTd = new Regex("<td[^>]+>([^{</td>}]+)</td>", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
		private static Regex rTorrentName = new Regex("<a href=\"http://exhentai.org/gallerytorrents.php?gid=([^&]+)&t=([^&]+)&gtid=([^\"+])\"[^>]+>([^<]+)</a>", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
		private static Regex rUploader = new Regex("<a href=\"http://exhentai.org/uploader/([^\"]+)\">", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);

		internal TorrentInfo(string body)
		{
			int i = 0;
			Match mTd = rTd.Match(body);
			Match m;

			string value;

			while (mTd.Success)
			{
				value = mTd.Groups[1].Value;

				switch (i++)
				{
					// Added
					case 0:
						this.PostedUTC = DateTime.Parse(value);
						this.Posted = this.PostedUTC.ToLocalTime();
						break;

					// Torrent Name
					case 1:
						m = rTorrentName.Match(value);
						this.Token = new GalleryToken(int.Parse(m.Groups[1].Value), m.Groups[2].Value);
						this.gtid = int.Parse(m.Groups[3].Value);
						this.FileName = m.Groups[4].Value;
						break;

					// Gallery
					case 2:
						break;

					// Size
					case 3:
						this.Size = parseSize(value);
						break;

					// Seeds
					case 4:
						this.Seeds = int.Parse(value);
						break;

					// Peers
					case 5:
						this.Peers = int.Parse(value);
						break;

					// DLs
					case 6:
						this.Downloads = int.Parse(value);
						break;

					// Uploader
					case 7:
						this.Uploader = rUploader.Match(value).Groups[1].Value;
						break;
				}
			}
		}

		//////////////////////////////////////////////////////////////////////////

		private static Regex rGtid		= new Regex("name=\"gtid\" value=\"(\\d+)\"",	RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
		private static Regex rPosted	= new Regex("Posted:</span>(.+)</td>",			RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
		private static Regex rSize		= new Regex("Size:</span>(.+)</td>",			RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
		private static Regex rSeeds		= new Regex("Seeds:</span>(.+)</td>",			RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
		private static Regex rPeers		= new Regex("Peers:</span>(.+)</td>",			RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
		private static Regex rDownload	= new Regex("Downloads:</span>(.+)</td>",		RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
		private static Regex rUPloader	= new Regex("Uploader:</span>(.+)</td>",		RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
		private static Regex rName		= new Regex("<a[^>]*>(.+)</a>",					RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);

		internal TorrentInfo(string body, GalleryToken token)
		{
			this.Token = token;

			this.gtid = int.Parse(rGtid.Match(body).Groups[1].Value);

			this.PostedUTC	= DateTime.Parse(rPosted.Match(body).Groups[1].Value);
			this.Posted		= this.PostedUTC.ToLocalTime();
			this.Size		= parseSize(rSize.Match(body).Groups[1].Value.Trim());
			this.Seeds		= int.Parse(rSeeds.Match(body).Groups[1].Value.Trim());
			this.Peers		= int.Parse(rPeers.Match(body).Groups[1].Value.Trim());
			this.Downloads	= int.Parse(rDownload.Match(body).Groups[1].Value.Trim());
			this.Uploader	= rUPloader.Match(body).Groups[1].Value.Trim();
			this.FileName	= rName.Match(body).Groups[1].Value.Trim();
		}

		private int parseSize(string value)
		{
			value = value.ToLower();
			int i = int.Parse(value.Substring(0, value.Length - 3));

			if (value.EndsWith("kb"))
				return i * 1024;

			if (value.EndsWith("mb"))
				return i * 1024 * 1024;

			if (value.EndsWith("gb"))
				return i * 1024 * 1024 * 1024;

			return i;
		}

		//////////////////////////////////////////////////////////////////////////

		public GalleryToken Token { get; internal set; }
		public int gtid { get; internal set; }

		public DateTime PostedUTC { get; internal set; }
		public DateTime Posted { get; internal set; }
		public int Size { get; internal set; }
		public int Seeds { get; internal set; }
		public int Peers { get; internal set; }
		public int Downloads { get; internal set; }
		public string Uploader { get; internal set; }
		public string FileName { get; internal set; }
	}
}
