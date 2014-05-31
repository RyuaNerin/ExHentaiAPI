using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExHentaiAPI
{
	public class GalleryFileSearchOption
	{
		public GalleryFileSearchOption(string path)
			: this(path, null)
		{
		}
		public GalleryFileSearchOption(string path, int? page)
		{
			this.Path = path;
			this.Page = page;
		}

		public string Path { get; private set; }
		public int? Page { get; set; }

		public bool UseSimilarityScan { get; set; }
		public bool OnlySearchCovers { get; set; }
		public bool ShowExpunged { get; set; }

		internal byte[] ToByteArray(out string boundary)
		{
			boundary = Helper.TempString();

			byte[] buff;

			FileInfo fin = new FileInfo(this.Path);

			using (MemoryStream ms = new MemoryStream())
			{
				using (StreamWriter sw = new StreamWriter(ms, Encoding.ASCII))
				{
					sw.Write("--{0}\r\n", boundary);
					sw.Write("Content-Disposition: form-data; name=\"f_sfile\"\r\n");
					sw.Write("\r\n");
					sw.Write("File Search\r\n");

					if (this.UseSimilarityScan)
					{
						sw.Write("--{0}\r\n", boundary);
						sw.Write("Content-Disposition: form-data; name=\"fs_similar\"\r\n");
						sw.Write("\r\n");
						sw.Write("on\r\n");
					}

					if (this.OnlySearchCovers)
					{
						sw.Write("--{0}\r\n", boundary);
						sw.Write("Content-Disposition: form-data; name=\"fs_covers\"\r\n");
						sw.Write("\r\n");
						sw.Write("on\r\n");
					}

					if (this.ShowExpunged)
					{
						sw.Write("--{0}\r\n", boundary);
						sw.Write("Content-Disposition: form-data; name=\"fs_exp\"\r\n");
						sw.Write("\r\n");
						sw.Write("on\r\n");
					}

					if (this.Page.HasValue && this.Page.Value > 1)
					{
						sw.Write("--{0}\r\n", boundary);
						sw.Write("Content-Disposition: form-data; name=\"page\"\r\n");
						sw.Write("\r\n");
						sw.Write(this.Page);
						sw.Write("\r\n");
					}

					sw.Write("--{0}\r\n", boundary);
					sw.Write("Content-Disposition: form-data; name=\"sfile\"; filename=\"\"\r\n", fin.Name);
					sw.Write("Content-Type: image/jpeg\r\n");
					sw.Write("\r\n");

					sw.Flush();

					int read;
					buff = new byte[4096];
					using (FileStream fstm = fin.OpenRead())
						while ((read = fstm.Read(buff, 0, 4096)) > 0)
							ms.Write(buff, 0, read);
					sw.Write("\r\n");


					sw.Write("--{0}", boundary);
					sw.Flush();

					buff = ms.ToArray();
				}
			}

			return buff;
		}
	}
}
