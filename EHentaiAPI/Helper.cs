using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ComputerBeacon.Json;

namespace ExHentaiAPI
{
	internal static class Helper
	{
		public static string ToCagetories(Categories category)
		{
			switch (category)
			{
				case Categories.ArtistCG:
					return "Artist CG";
				case Categories.AsianPorn:
					return "Asian Porn";
				case Categories.Cosplay:
					return "Cosplay";
				case Categories.Doujinshi:
					return "Doujinshi";
				case Categories.GameCG:
					return "Game CG";
				case Categories.ImageSets:
					return "Image Sets";
				case Categories.Manga:
					return "Manga";
				case Categories.Misc:
					return "Mics";
				case Categories.NonH:
					return "Non H";
				case Categories.Western:
					return "Westorn";		
				default:
					return null;
			}
		}
		public static Categories ToCategories(string categoryString)
		{
			switch (categoryString)
			{
				case "Doujinshi":
					return Categories.Doujinshi;
				case "Manga":
					return Categories.Manga;
				case "Artist CG Sets":
					return Categories.ArtistCG;
				case "Game CG Sets":
					return Categories.GameCG;
				case "Western":
					return Categories.Western;
				case "Image Sets":
					return Categories.ImageSets;
				case "Non-H":
					return Categories.NonH;
				case "Cosplay":
					return Categories.Cosplay;
				case "Asian Porn":
					return Categories.AsianPorn;
				case "Misc":
					return Categories.Misc;
				case "Private":
					return Categories.Private;
			}

			return Categories.None;
		}

		public static int ContainsCategory(Categories category, Categories find)
		{
			if ((category & find) == find)
				return 1;
			else
				return 0;
		}

		private static DateTime _date19700101 = new DateTime(1970, 1, 1, 0, 0, 0);
		public static DateTime ToDateTime(int date)
		{
			return Helper._date19700101.AddSeconds(date);
		}

		public static object getJsonData(JsonObject jo, string key)
		{
			if (jo.ContainsKey(key))
				return jo[key];
			else
				return null;
		}

		private static Random _random = new Random(DateTime.UtcNow.Millisecond);
		public static Random Random { get { return Helper._random; }}

		private static int _boundaryLength = 30;
		private static char[] _boundaryChars =
		{
 			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
 			'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
 			'u', 'v', 'w', 'x', 'y', 'z',
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
			'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
			'U', 'V', 'W', 'X', 'Y', 'Z',
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
		};
		public static string TempString()
		{
			StringBuilder stringBuilder = new StringBuilder(Helper._boundaryLength);

			for (int i = 0; i < Helper._boundaryLength; ++i)
				stringBuilder.Append(Helper._boundaryChars[Helper._random.Next(0, Helper._boundaryChars.Length - 1)]);

			return stringBuilder.ToString();
		}

		public static string StringFromStream(Stream stream)
		{
			string r;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] buff = new byte[4096];
				int read;

				while ((read = stream.Read(buff, 0, 4096)) > 0)
					memoryStream.Write(buff, 0, read);
				memoryStream.Flush();

				r = Encoding.UTF8.GetString(memoryStream.ToArray());

				memoryStream.Dispose();
			}

			return r;
		}
	}
}
