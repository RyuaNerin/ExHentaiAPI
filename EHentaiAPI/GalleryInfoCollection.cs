using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ComputerBeacon.Json;

namespace ExHentaiAPI
{
	public class GalleryInfoCollection : List<GalleryInfo>
	{
		public GalleryInfoCollection()
			: base()
		{
		}
		public GalleryInfoCollection(IEnumerable<GalleryInfo> collection)
			: base(collection)
		{
		}

		internal GalleryInfoCollection(string body)
		{
			JsonArray ja = (JsonArray)((new JsonObject(body))["gmetadata"]);

			for (int i = 0; i < ja.Count; i++)
				this.Add(new GalleryInfo((JsonObject)ja[i]));
		}
	}
}
