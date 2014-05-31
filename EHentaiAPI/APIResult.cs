using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;

namespace ExHentaiAPI
{
	internal class ApiResult : IAsyncResult
	{
		public ApiResult(WebRequest webRequest, int id, AsyncCallback callBack, object userState, object state)
			: this(webRequest, null, id, callBack, userState)
		{
			this.m_userState = state;
		}
		public ApiResult(WebRequest webRequest, byte[] buff, int id, AsyncCallback callBack, object userState, object state)
			: this(webRequest, buff, id, callBack, userState)
		{
			this.m_userState = state;
		}
		public ApiResult(WebRequest webRequest, int id, AsyncCallback callBack, object userState)
			: this(webRequest, null, id, callBack, userState)
		{
		}
		public ApiResult(WebRequest webRequest, byte[] buff, int id, AsyncCallback callBack, object userState)
		{
			this.m_callback = callBack;
			this.m_userState = userState;
			this.m_parentId = id;
			this.m_req = webRequest;

			this.m_manualEvent = new ManualResetEvent(false);

			this.AsyncState = userState;
			this.IsCompleted = false;
			this.CompletedSynchronously = false;

			this.Cancel = false;

			this.m_buff = buff;

			if (buff == null)
				this.m_req.BeginGetResponse(this.ResponseCallback, null);
			else
				this.m_req.BeginGetRequestStream(this.RequestCallback, null);
		}

		~ApiResult()
		{
			this.m_manualEvent.Close();
		}

		private void RequestCallback(IAsyncResult asyncResult)
		{
			Stream stream = this.m_req.EndGetRequestStream(asyncResult);
			stream.Write(this.m_buff, 0, this.m_buff.Length);
			stream.Flush();
			stream.Close();

			this.m_buff = null;

			this.m_req.BeginGetResponse(this.ResponseCallback, null);
		}

		private void ResponseCallback(IAsyncResult asyncResult)
		{
			if (this.Cancel)
			{
				this.m_manualEvent.Set();
			}
			else
			{
				HttpWebResponse hwRes = this.m_req.EndGetResponse(asyncResult) as HttpWebResponse;

				if (this.m_userState is bool)
					this.m_body = hwRes.Headers.Get("set-cookie");
				else
					this.m_body = Helper.StringFromStream(hwRes.GetResponseStream());

				hwRes.Close();

				this.IsCompleted = true;

				this.m_manualEvent.Set();

				if (this.m_callback != null)
					this.m_callback.Invoke(this);
			}
		}

		public ManualResetEvent	m_manualEvent;
		public WebRequest		m_req;
		public AsyncCallback	m_callback;
		public int				m_parentId;
		public object			m_userState;
		public string			m_body;
		public byte[]			m_buff;

		public bool			Cancel					{ get; set; }

		public WaitHandle	AsyncWaitHandle			{ get { return this.m_manualEvent; } }
		public bool			IsCompleted				{ get; set; }
		public object		AsyncState				{ get; set; }
		public bool			CompletedSynchronously	{ get; set; }
	}
}
