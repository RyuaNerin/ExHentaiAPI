using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExHentaiAPI
{
	public class GallerySearchOption
	{
		public GallerySearchOption()
			: this("Search Keywords", null, Categories.All)
		{
		}
		public GallerySearchOption(string keyword)
			: this(keyword, null, Categories.All)
		{
		}
		public GallerySearchOption(string keyword, int? page)
			: this(keyword, page, Categories.All)
		{
		}
		public GallerySearchOption(string keyword, int? page, Categories category)
		{
			this.Keyword = keyword;
			this.Page = page;
			this.Category = category;

			this.SearchGalleryName = true;
			this.SearchGalleryTags = true;
			this.SearchTorrentFileNames = true;
			this.SearchLowPowerTags = true;
		}
		public string Keyword { get; set; }
		public Categories Category { get; set; }
		public bool AdvencedSearch { get; set; }
		public bool SearchGalleryName { get; set; }
		public bool SearchGalleryTags { get; set; }
		public bool SearchGalleryDescription { get; set; }
		public bool SearchTorrentFileNames { get; set; }
		public bool OnlyShowGalleriesWithTorrent { get; set; }
		public bool SearchLowPowerTags { get; set; }
		public bool SearchDownvotedTags { get; set; }
		public bool ShowExpungedGalleries { get; set; }
		public bool MinimumRating { get; set; }
		public int MinimumRatingValue { get; set; }
		public int? Page { get; set; }

		internal string GetParametor()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('?');
			if (this.Page.HasValue && this.Page.Value > 0)
				sb.AppendFormat("page={0}&", this.Page.Value);
			sb.AppendFormat("f_doujinshi={0}&", Helper.ContainsCategory(this.Category, Categories.Doujinshi));
			sb.AppendFormat("f_manga={0}&", Helper.ContainsCategory(this.Category, Categories.Manga));
			sb.AppendFormat("f_artistcg={0}&", Helper.ContainsCategory(this.Category, Categories.ArtistCG));
			sb.AppendFormat("f_gamecg={0}&", Helper.ContainsCategory(this.Category, Categories.GameCG));
			sb.AppendFormat("f_western={0}&", Helper.ContainsCategory(this.Category, Categories.Western));
			sb.AppendFormat("f_non-h={0}&", Helper.ContainsCategory(this.Category, Categories.NonH));
			sb.AppendFormat("f_imageset={0}&", Helper.ContainsCategory(this.Category, Categories.ImageSets));
			sb.AppendFormat("f_cosplay={0}&", Helper.ContainsCategory(this.Category, Categories.Cosplay));
			sb.AppendFormat("f_asianporn={0}&", Helper.ContainsCategory(this.Category, Categories.AsianPorn));
			sb.AppendFormat("f_misc={0}&", Helper.ContainsCategory(this.Category, Categories.Misc));

			sb.AppendFormat("f_search={0}&", Uri.EscapeDataString(this.Keyword));
			sb.AppendFormat("f_apply=Apply+Filter&");

			if (this.AdvencedSearch)
			{
				sb.Append("advsearch=1&");

				if (this.SearchGalleryName)
					sb.AppendFormat("f_sname=on&");

				if (this.SearchGalleryTags)
					sb.AppendFormat("f_stags=on&");

				if (this.SearchGalleryDescription)
					sb.AppendFormat("f_sdesc=on&");

				if (this.SearchTorrentFileNames)
					sb.AppendFormat("f_storr=on&");

				if (this.OnlyShowGalleriesWithTorrent)
					sb.AppendFormat("f_sto=on&");

				if (this.SearchLowPowerTags)
					sb.AppendFormat("f_sdt1=on&");

				if (this.SearchDownvotedTags)
					sb.AppendFormat("f_sdt2=on&");

				if (this.ShowExpungedGalleries)
					sb.AppendFormat("f_sh=on&");

				if (this.MinimumRating)
					sb.AppendFormat("f_sr=on&f_srdd={0}&", this.MinimumRating);

				sb.AppendFormat("f_srdd={0}&", this.MinimumRatingValue);
			}

			sb.Remove(sb.Length - 1, 1);

			return sb.ToString();
		}
	}
}
