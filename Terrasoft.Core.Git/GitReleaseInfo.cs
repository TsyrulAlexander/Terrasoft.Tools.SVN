using System.Collections.Generic;
using Newtonsoft.Json;

namespace Terrasoft.Core.Git
{
    public class GitReleaseInfo
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("tag_name")] public string TagName { get; set; }

        [JsonProperty("assets")] public IEnumerable<GitAssetInfo> Assets { get; set; }
    }
}