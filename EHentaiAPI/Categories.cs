using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExHentaiAPI
{
	[Flags]
	public enum Categories : short
	{
		None			= 0x000,
		Doujinshi		= 0x001,
		Manga			= 0x002,
		ArtistCG		= 0x004,
		GameCG			= 0x008,
		Western			= 0x010,
		NonH			= 0x020,
		ImageSets		= 0x040,
		Cosplay			= 0x080,
		AsianPorn		= 0x100,
		Misc			= 0x200,
		Private			= 0x400,
		All				= 0x7FF
	}
}
