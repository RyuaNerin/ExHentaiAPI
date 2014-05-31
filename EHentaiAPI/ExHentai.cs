using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ComputerBeacon.Json;
using System.Net;

namespace ExHentaiAPI
{
	public class ExHentai
	{
		public ExHentai()
		{
			this.m_ipbMemberID = null;
			this.m_ipbPassHash = null;

			this.Proxy = WebRequest.DefaultWebProxy;
		}
		
		public ExHentai(string ipbMemberId, string ipbPassHash)
		{
			this.ipbMemberId = ipbMemberId;
			this.ipbPassHash = ipbPassHash;
		}

		#region Properties
		internal string m_cookie;

		private string m_ipbMemberID;
		public string ipbMemberId
		{
			get { return this.m_ipbMemberID; }
			set
			{
				this.m_ipbMemberID = value;
				this.m_cookie = String.Format("ipb_member_id={0}; ipb_pass_hash={1};", this.m_ipbMemberID, this.m_ipbPassHash);
			}
		}

		private string m_ipbPassHash;
		public string ipbPassHash
		{
			get { return this.m_ipbPassHash; }
			set
			{
				this.m_ipbPassHash = value;
				this.m_cookie = String.Format("ipb_member_id={0}; ipb_pass_hash={1};", this.m_ipbMemberID, this.m_ipbPassHash);
			}
		}

		public IWebProxy Proxy { get; set; }
		#endregion

		//////////////////////////////////////////////////////////////////////////

		public void CancleAsync(IAsyncResult asyncResult)
		{
			ApiResult apiResult = asyncResult as ApiResult;
			if (apiResult == null)
				throw new FormatException();

			apiResult.Cancel = true;
			apiResult.CompletedSynchronously = true;
			apiResult.IsCompleted = true;

			apiResult.m_req.Abort();
		}

		//////////////////////////////////////////////////////////////////////////

		private const int KeyLogin = 0;
		#region Login
		public void Login(string id, string pw)
		{
			IAsyncResult asyncResult = this.BeginLogin(id, pw, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			this.EndLogin(asyncResult);
		}
		#endregion

		#region LoginAsync
		public IAsyncResult BeginLogin(string id, string pw, AsyncCallback callBack)
		{
			return this.BeginLogin(id, pw, callBack, null);
		}
		public IAsyncResult BeginLogin(string id, string pw, AsyncCallback callBack, object userState)
		{
			byte[] buff = Encoding.ASCII.GetBytes(
				String.Format("returntype=8&CookieDate=1&b=d&bt=pone&UserName={0}&PassWord={1}&ipb_login_submit=Login%21", id, pw));

			HttpWebRequest wReq = WebRequest.Create("https://forums.e-hentai.org/index.php?act=Login&CODE=01") as HttpWebRequest;
			wReq.Proxy = this.Proxy;
			wReq.Method = "POST";
			wReq.Referer = "http://e-hentai.org/";
			wReq.ContentType = "application/x-www-form-urlencoded";

			return new ApiResult(wReq, buff, ExHentai.KeyLogin, callBack, userState, true);
		}

		private static Regex rID = new Regex("ipb_member_id=([^;]+);", RegexOptions.Compiled);
		private static Regex rPW = new Regex("ipb_pass_hash=([^;]+);", RegexOptions.Compiled);
		public void EndLogin(IAsyncResult asyncResult)
		{
			ApiResult aRes = asyncResult as ApiResult;
			if (aRes == null || aRes.m_parentId != ExHentai.KeyLogin)
				throw new FormatException();

			aRes.CompletedSynchronously = true;

			this.ipbMemberId = rID.Match(aRes.m_body).Groups[1].Value;
			this.ipbPassHash = rPW.Match(aRes.m_body).Groups[1].Value;
		}
		#endregion

		//////////////////////////////////////////////////////////////////////////

		private const int KeyGalleryToken = 1;
		#region GetGalleryTokens
		public GalleryTokenCollection GetGalleryTokens()
		{
			IAsyncResult asyncResult = this.BeginGetGalleryTokens(null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetGalleryTokens(asyncResult);
		}
		public GalleryTokenCollection GetGalleryTokens(GallerySearchOption options)
		{
			IAsyncResult asyncResult = this.BeginGetGalleryTokens(options, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetGalleryTokens(asyncResult);
		}
		public GalleryTokenCollection GetGalleryTokens(GalleryFileSearchOption options)
		{
			IAsyncResult asyncResult = this.BeginGetGalleryTokens(options, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetGalleryTokens(asyncResult);
		}
		#endregion

		#region GetGalleryTokensAsync
		public IAsyncResult BeginGetGalleryTokens(AsyncCallback callBack)
		{
			return this.BeginGetGalleryTokens(callBack, null);
		}
		public IAsyncResult BeginGetGalleryTokens(AsyncCallback callBack, object userState)
		{
			WebRequest wReq = WebRequest.Create("http://g.e-hentai.org/archiver.php");
			wReq.Proxy = this.Proxy;
			wReq.Method = "GET";
			wReq.Headers.Set("cookie", this.m_cookie);

			return new ApiResult(wReq, ExHentai.KeyGalleryToken, callBack, userState);
		}

		public IAsyncResult BeginGetGalleryTokens(GallerySearchOption options, AsyncCallback callBack)
		{
			return this.BeginGetGalleryTokens(options, callBack, null);
		}
		public IAsyncResult BeginGetGalleryTokens(GallerySearchOption options, AsyncCallback callBack, object userState)
		{
			WebRequest wReq = WebRequest.Create(String.Format("http://exhentai.org/{0}", options.GetParametor()));
			wReq.Proxy = this.Proxy;
			wReq.Method = "GET";
			wReq.Headers.Set("cookie", this.m_cookie);

			return new ApiResult(wReq, ExHentai.KeyGalleryToken, callBack, userState);
		}

		public IAsyncResult BeginGetGalleryTokens(GalleryFileSearchOption options, AsyncCallback callBack)
		{
			return this.BeginGetGalleryTokens(options, callBack, null);
		}
		public IAsyncResult BeginGetGalleryTokens(GalleryFileSearchOption options, AsyncCallback callBack, object userState)
		{
			string boundary;
			byte[] buff = options.ToByteArray(out boundary);

			WebRequest wReq = WebRequest.Create("http://ul.exhentai.org/image_lookup.php");
			wReq.Proxy = this.Proxy;
			wReq.Method = "POST";
			wReq.Headers.Set("cookie", this.m_cookie);
			wReq.ContentType = String.Format("multipart/form-data; boundary={0}", boundary);

			return new ApiResult(wReq, buff, ExHentai.KeyGalleryToken, callBack, userState);
		}

		public GalleryTokenCollection EndGetGalleryTokens(IAsyncResult asyncResult)
		{
			ApiResult aRes = asyncResult as ApiResult;
			if (aRes == null || aRes.m_parentId != ExHentai.KeyGalleryToken)
				throw new FormatException();

			aRes.CompletedSynchronously = true;

			return new GalleryTokenCollection(aRes.m_body);
		}
		#endregion

		//////////////////////////////////////////////////////////////////////////
		
		private const int KeyGalleryInfo = 2;
		#region GetGalleryInfo
		public GalleryInfoCollection GetGalleryInfo(GalleryTokenCollection collection)
		{
			IAsyncResult asyncResult = this.BeginGetGalleryInfo(collection, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetGalleryInfo(asyncResult);
		}
		public GalleryInfoCollection GetGalleryInfo(GalleryTokenCollection collection, int offset, int length)
		{
			IAsyncResult asyncResult = this.BeginGetGalleryInfo(collection, offset, length, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetGalleryInfo(asyncResult);
		}
		public GalleryInfoCollection GetGalleryInfo(GalleryToken token)
		{
			IAsyncResult asyncResult = this.BeginGetGalleryInfo(token, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetGalleryInfo(asyncResult);
		}
		#endregion

		#region GetGalleryInfoAsync
		public IAsyncResult BeginGetGalleryInfo(GalleryTokenCollection collection, AsyncCallback callBack)
		{
			return this.BeginGetGalleryInfo(collection.ToJsonArray(), callBack, null);
		}
		public IAsyncResult BeginGetGalleryInfo(GalleryTokenCollection collection, AsyncCallback callBack, object userState)
		{
			return this.BeginGetGalleryInfo(collection.ToJsonArray(), callBack, userState);
		}

		public IAsyncResult BeginGetGalleryInfo(GalleryTokenCollection collection, int offset, int length, AsyncCallback callBack)
		{
			return this.BeginGetGalleryInfo(collection.ToJsonArray(offset, length), callBack, null);
		}
		public IAsyncResult BeginGetGalleryInfo(GalleryTokenCollection collection, int offset, int length, AsyncCallback callBack, object userState)
		{
			return this.BeginGetGalleryInfo(collection.ToJsonArray(offset, length), callBack, userState);
		}

		public IAsyncResult BeginGetGalleryInfo(GalleryToken token, AsyncCallback callBack)
		{
			return this.BeginGetGalleryInfo(token.ToByteArray(), callBack, null);
		}
		public IAsyncResult BeginGetGalleryInfo(GalleryToken token, AsyncCallback callBack, object userState)
		{
			return this.BeginGetGalleryInfo(token.ToByteArray(), callBack, userState);
		}
		private IAsyncResult BeginGetGalleryInfo(byte[] buff, AsyncCallback callBack, object userState)
		{
			WebRequest wReq = WebRequest.Create("http://exhentai.org/api.php");
			wReq.Proxy = this.Proxy;
			wReq.Method = "POST";
			wReq.Headers.Set("cookie", this.m_cookie);
			wReq.ContentType = "application/json";

			return new ApiResult(wReq, buff, ExHentai.KeyGalleryInfo, callBack, userState);
		}

		public GalleryInfoCollection EndGetGalleryInfo(IAsyncResult asyncResult)
		{
			ApiResult aRes = asyncResult as ApiResult;
			if (aRes == null || aRes.m_parentId != ExHentai.KeyGalleryInfo)
				throw new FormatException();

			aRes.CompletedSynchronously = true;

			return new GalleryInfoCollection(aRes.m_body);
		}
		#endregion

		//////////////////////////////////////////////////////////////////////////
		
		private const int KeyArchiveLink = 3;
		#region GetArchiveLink
		public string GetArchiveLink(GalleryInfo info)
		{
			IAsyncResult asyncResult = this.BeginGetArchiveLink(info, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetArchiveLink(asyncResult);
		}
		#endregion

		#region GetArchiveLinkAsync
		private static byte[] ArchiveLinkData = Encoding.ASCII.GetBytes("dlcheck=Download%20Archive");
		public IAsyncResult BeginGetArchiveLink(GalleryInfo info, AsyncCallback callBack)
		{
			return this.BeginGetArchiveLink(info, callBack, null);
		}
		public IAsyncResult BeginGetArchiveLink(GalleryInfo info, AsyncCallback callBack, object userState)
		{
			if (info.ArchiverKeyTime < DateTime.UtcNow)
				throw new Exception("Archiver Key Time is expired.");

			string url = String.Format(
						"http://exhentai.org/archiver.php?gid={0}&token={1}&or={2}",
						info.token.gid,
						info.token.token,
						info.ArchiverKey);

			HttpWebRequest wReq = WebRequest.Create(url) as HttpWebRequest;
			wReq.Proxy = this.Proxy;
			wReq.Method = "POST";
			wReq.Headers.Set("cookie", this.m_cookie);
			wReq.Referer = url;
			wReq.ContentType = "application/x-www-form-urlencoded";

			return new ApiResult(wReq, ArchiveLinkData, ExHentai.KeyArchiveLink, callBack, userState);
		}
		private static Regex rGetArchiveLink_Link = new Regex("<a href=\"(http[^\"]*)\"", RegexOptions.Compiled);
		public string EndGetArchiveLink(IAsyncResult asyncResult)
		{
			ApiResult aRes = asyncResult as ApiResult;
			if (aRes == null || aRes.m_parentId != ExHentai.KeyArchiveLink)
				throw new FormatException();

			aRes.CompletedSynchronously = true;

			Match m = ExHentai.rGetArchiveLink_Link.Match(aRes.m_body);

			if (m.Success)
				return m.Groups[1].Value + "?start=1";
			else
				return null;
		}
		#endregion

		//////////////////////////////////////////////////////////////////////////
		
		private const int KeyGalleryTokenFormURL = 4;
		#region GetGalleryTokenFromURL
		public GalleryToken GetGalleryTokenFromURL(string url)
		{
			IAsyncResult asyncResult = this.BeginGetGalleryTokenFromURL(url, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetGalleryTokenFromURL(asyncResult);
		}
		#endregion

		#region GetGalleryTokenFromURLAsync
		private static Regex rGetGalleryInfoFromImageG = new Regex("http://g.e-hentai.org/s/([^/]+)/([^-]+)-(.+)");
		private static Regex rGetGalleryInfoFromImageE = new Regex("http://exhentai.org/s/([^/]+)/([^-]+)-(.+)");
		public IAsyncResult BeginGetGalleryTokenFromURL(string url, AsyncCallback callBack)
		{
			return this.BeginGetGalleryTokenFromURL(url, callBack, null);
		}
		public IAsyncResult BeginGetGalleryTokenFromURL(string url, AsyncCallback callBack, object userState)
		{
			Match m = ExHentai.rGetGalleryInfoFromImageG.Match(url);

			if (!m.Success)
			{
				m = ExHentai.rGetGalleryInfoFromImageE.Match(url);
				if (!m.Success)
					throw new FormatException();
			}

			byte[] buff = Encoding.ASCII.GetBytes(
				String.Format(
					"{0} \"method\": \"gtoken\", \"pagelist\": [[{1}. \"{2}\", {3}]] {4}",
					"{",
					m.Groups[2].Value,
					m.Groups[1].Value,
					m.Groups[3].Value,
					"}"
					));

			WebRequest wReq = WebRequest.Create("http://g.e-hentai.org/api.php");
			wReq.Proxy = this.Proxy;
			wReq.Method = "POST";
			wReq.Headers.Set("cookie", this.m_cookie);
			wReq.ContentType = "application/json";

			return new ApiResult(wReq, buff, ExHentai.KeyGalleryTokenFormURL, callBack, userState);
		}

		public GalleryToken EndGetGalleryTokenFromURL(IAsyncResult asyncResult)
		{
			ApiResult aRes = asyncResult as ApiResult;
			if (aRes == null || aRes.m_parentId != ExHentai.KeyGalleryTokenFormURL)
				throw new FormatException();

			aRes.CompletedSynchronously = true;

			JsonObject jo = (JsonObject)((JsonArray)((new JsonObject(aRes.m_body))["gmetadata"]))[0];

			return new GalleryToken((int)jo["gid"], (string)jo["token"]);
		}
		#endregion

		//////////////////////////////////////////////////////////////////////////
		
		private const int KeyTorrentInfo = 5;
		#region GetTorrentInfo
		public TorrentInfoCollection GetTorrentInfo(GalleryToken token)
		{
			IAsyncResult asyncResult = this.BeginGetTorrentInfo(token, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetTorrentInfo(asyncResult);
		}
		public TorrentInfoCollection GetTorrentInfo(TorrentSearchOption option)
		{
			IAsyncResult asyncResult = this.BeginGetTorrentInfo(option, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetTorrentInfo(asyncResult);
		}
		#endregion

		#region GetTorrentInfoAsync
		public IAsyncResult BeginGetTorrentInfo(TorrentSearchOption option, AsyncCallback callBack)
		{
			return this.BeginGetTorrentInfo(option, callBack, null);
		}
		public IAsyncResult BeginGetTorrentInfo(TorrentSearchOption option, AsyncCallback callBack, object userState)
		{
			WebRequest wReq = WebRequest.Create(option.GetUrl());
			wReq.Proxy = this.Proxy;
			wReq.Method = "GET";
			wReq.Headers.Set("cookie", this.m_cookie);

			return new ApiResult(wReq, ExHentai.KeyTorrentInfo, callBack, userState);
		}
		public IAsyncResult BeginGetTorrentInfo(GalleryToken token, AsyncCallback callBack)
		{
			return this.BeginGetTorrentInfo(token, callBack, null);
		}
		public IAsyncResult BeginGetTorrentInfo(GalleryToken token, AsyncCallback callBack, object userState)
		{
			WebRequest wReq = WebRequest.Create(String.Format("http://exhentai.org/gallerytorrents.php?gid={0}&t={1}", token.gid, token.token));
			wReq.Proxy = this.Proxy;
			wReq.Method = "GET";
			wReq.Headers.Set("cookie", this.m_cookie);

			return new ApiResult(wReq, ExHentai.KeyTorrentInfo, callBack, userState);
		}

		public TorrentInfoCollection EndGetTorrentInfo(IAsyncResult asyncResult)
		{
			ApiResult aRes = asyncResult as ApiResult;
			if (aRes == null || aRes.m_parentId != ExHentai.KeyTorrentInfo)
				throw new FormatException();

			aRes.CompletedSynchronously = true;

			GalleryToken token = aRes.m_userState as GalleryToken;

			if (token == null)
				return new TorrentInfoCollection(aRes.m_body);
			else
				return new TorrentInfoCollection(aRes.m_body, token);
		}
		#endregion

		//////////////////////////////////////////////////////////////////////////

		private const int KeyTorrentUrl = 6;
		#region GetTorrentURL
		public string GetTorrentURL(TorrentInfo info)
		{
			IAsyncResult asyncResult = this.BeginGetTorrentURL(info, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			return this.EndGetTorrentURL(asyncResult);
		}
		#endregion

		#region GetTorrentURLAsync
		public IAsyncResult BeginGetTorrentURL(TorrentInfo info, AsyncCallback callBack)
		{
			return this.BeginGetTorrentURL(info, callBack, null);
		}
		public IAsyncResult BeginGetTorrentURL(TorrentInfo info, AsyncCallback callBack, object userState)
		{
			WebRequest wReq = WebRequest.Create(
				String.Format("http://exhentai.org/gallerytorrents.php?gid={0}&t={1{&gtid={2}",
					info.Token.gid,
					info.Token.token,
					info.gtid));
			wReq.Proxy = this.Proxy;
			wReq.Method = "GET";
			wReq.Headers.Set("cookie", this.m_cookie);

			return new ApiResult(wReq, ExHentai.KeyTorrentUrl, callBack, userState);
		}

		private static Regex rLink = new Regex("<a href=\"(http://ehtracker.org/t/.+.torrent)\"");
		public string EndGetTorrentURL(IAsyncResult asyncResult)
		{
			ApiResult aRes = asyncResult as ApiResult;
			if (aRes == null || aRes.m_parentId != ExHentai.KeyTorrentUrl)
				throw new FormatException();

			aRes.CompletedSynchronously = true;

			return rLink.Match(aRes.m_body).Groups[1].Value;
		}
		#endregion
	}
}