# ExHentaiAPI 1.0.3.0
* ExHentai API Library
* Written by .Net Framework 3.5
* CopyRight (C) 2014, RyuaNerin


# OpenSource Library
* [JsonTookit 4.1.736 (Ms-PL)](http://jsontoolkit.codeplex.com/)


# [LICENSE](/LICENSE)
* MIT LICENSE
* *EXEMPTION CLAUSE*
 * *All caused by the usage of WARP is the responsibility of the user.*
 * *Code contributors WARP is not responsible for the use.*


# CLASS
## Categories (short enum Flags)
* 0x0000 `None`
* 0x0001 `Douhinshi`
* 0x0002 `Manga`
* 0x0004 `ArtistCG`
* 0x0008 `GameCG`
* 0x0010 `WEstern`
* 0x0020 `NonH`
* 0x0040 `ImageSets`
* 0x0080 `Cosplay`
* 0x0100 `AsianPron`
* 0x0200 `Misc`
* 0x0400 `Private`
* 0x07FF `All`


## GallerySearchOption
* Constructor
	* `GallerySearchOption()`
	* `GallerySearchOption(keyword)`
	* `GallerySearchOption(keyword, int? page)`
	* `GallerySearchOption(keyword, int? page, Categories)`


* Property
	* `string Keyword`
	* `Categories Category`
	* `int? Page`
	* `bool AdvencedSearch`
	* `bool SearchGalleryName`
	* `bool SearchGalleryTags`
	* `bool SearchGalleryDescription`
	* `bool SearchTorrentFileNames`
	* `bool OnlyShowGalleriesWithTorrent`
	* `bool SearchLowPowerTags`
	* `bool SearchDownvotedTags`
	* `bool ShowExpungedGalleries`
	* `bool MinimumRating`
	* `int MinimumRatingValue`

## GalleryFileSearchOption
* Constructor
	* `GalleryFileSearchOption(path)`
	* `GalleryFileSearchOption(path, int? page)`


* Property
	* `string Path` *(ReadOnly)*
	* `int? Page`
	* `bool UseSimilarityScan`
	* `bool OnlySearchCovers`
	* `bool ShowExpunged`


## TorrentSearchOption
* Constructor
	* `TorrentSearchOption()`
	* `TorrentSearchOption(keyword)`
	* `TorrentSearchOption(keyword, int? page)`


* Property
	* `string Keyword`
	* `int? Page`
	* `bool Ascending`
	* `OrderTypes OderType`
		* `Added`
		* `Size`
		* `Seeds`
		* `Peers`
		* `Downloads`


## GalleryToken
* Constructor
	* `GalleryToken(int gid, string token)`


* Property
	* `int gid`
	* `string token`


## GalleryTokenCollection : List
* Constructor
	* `GalleryTokenCollection()`
	* `GalleryTokenCollection(IEnumerable)`


* Property
	* `int ShowingStart`
	* `int ShowingEnd`
	* `int ShowingCount`


## GalleryInfo
* Property *(ReadOnly)*
	* `GalleryToken token`
	* `string ArchiverKey`
	* `DateTime ArchiveKeyTimeUTC`
	* `DateTime ArchiveKeyTime`
	* `string Title`
	* `string TitleJpn`
	* `Categories Category`
	* `string ThumbURL`
	* `string ThumbPath`
	* `string Uploader`
	* `DateTime PostedUTC`
	* `DateTime Posted`
	* `int FileCount`
	* `int FileSize`
	* `bool Expunged`
	* `float Rating`
	* `int TorrentCount`
	* `string error`
	* `string[] Tags`


## GalleryInfoCollection : List
* Constructor
	* `GalleryInfoCollection()`
	* `GalleryInfoCollection(IEnumerable)`


## TorrentInfo
* Property
	* `GalleryToken token`
	* `int gtid`
	* `DateTime PostedUTC`
	* `DateTime Posted`
	* `int Size`
	* `int Seeds`
	* `int Peers`
	* `int Downloads`
	* `string UPloader`
	* `string FileName`


## TorrentInfoCollection : List
* Constructor
	* `TorrentInfoCollection()`
	* `TorrentInfoCollection(IEnumerable)`


* Property
	* `int ShowingStart`
	* `int ShowingEnd`
	* `int ShowingCount`


## ExHentai
* Constructor
	* `ExHentai()`
	* `ExHentai(ipdMemberId, ipbPassHash)`


* Property
	* `string ipbMemberId`
	* `string ipbPashHash`
	* `IWebProxy Proxy`


* Functions
	* *All functions surpport Async*
	* void Login
		* `Login(id, pw)`
	* GalleryTokenCollection GetGalleryTokens
		* `GetGalleryTokens()`
		* `GetGalleryTokens(GallerySearchOption)`
		* `GetGalleryTokens(GalleryFileSearchOption)`
	* GalleryInfoCollection GetGalleryInfo
		* `GetGalleryInfo(GalleryTokenCollection)`
		* `GetGalleryInfo(GalleryTokenCollection, int offset, int length)`
		* `GetGalleryInfo(GalleryToken)`
	* String GetArchiveLink
		* `String GetArchiveLink(GalleryInfo)`
	* GalleryToken GetGalleryTokenFromURL
		* `GetGalleryTokenFromURL(string)`
	* TorrentInfoCollection GetTorrentInfo
		* `GetTorrentInfo(GalleryToken)`
		* `GetTorrentInfo(TorrentSearchOption)`
	* string GetTorrentURL
		* `GetTorrentURL(TorrentInfo)`