using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Terrasoft.Core.Git
{
	public class GitReleaseInfo {
		[JsonProperty("url")]
		public string Url { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("tag_name")]
		public string TagName { get; set; }
		[JsonProperty("assets")]
		public IEnumerable<GitAssetInfo> Assets { get; set; }
	}
}
