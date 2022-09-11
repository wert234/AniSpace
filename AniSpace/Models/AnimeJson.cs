using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models
{
    class Root
    {
        [JsonProperty("id")]
        public decimal id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("russian")]
        public string russian { get; set; }
        [JsonProperty("image")]
        public Image image { get; set; }
        [JsonProperty("url")]
        public Uri url { get; set; }
        [JsonProperty("kind")]
        public string kind { get; set; }
        [JsonProperty("score")]
        public string score { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("episodes")]
        public int episodes { get; set; }
        [JsonProperty("episodes_aired")]
        public int episodes_aired { get; set; }
        [JsonProperty("aired_on")]
        public string aired_on { get; set; }
        [JsonProperty("released_on")]
        public string released_on { get; set; }
    }


    public class Image
    {
        [Newtonsoft.Json.JsonProperty("original")]
        public string original { get; set; }
        [Newtonsoft.Json.JsonProperty("preview")]
        public string preview { get; set; }
        [Newtonsoft.Json.JsonProperty("x96")]
        public string x96 { get; set; }
        [Newtonsoft.Json.JsonProperty("x48")]
        public string x48 { get; set; }
    }
}
