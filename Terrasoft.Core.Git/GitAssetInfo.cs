using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Terrasoft.Core.Git {
	public class GitAssetInfo {
		[JsonProperty("url")]
		public string Url { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("content_type")]
		public string ContentType { get; set; }
		[JsonProperty("download_url")]
		public string DownloadUrl { get; set; }
	}
}
