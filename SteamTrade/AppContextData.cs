using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SteamTrade
{
    /// <summary>
    /// AppContextData is retrieved from the html page downloaded upon trade start
    /// </summary>
    public class AppContextData
    {
        //addtional scraping of data variables needed
        public AppContextDataKeyedCollection Apps { get; set; }
        //public Dictionary<int, App> Apps { get; set; }
        public bool IsTradePartnerOnProbation { get; set; }
        public string InventoryLoadUrl { get; set; }

        public class App
        {
            [JsonProperty("appid")]
            public int AppId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }

            [JsonProperty("asset_count")]
            public int AssetCount { get; set; }

            [JsonProperty("inventory_logo")]
            public string InventoryLogo { get; set; }

            [JsonProperty("trade_permissions")]
            public string TradePermissions { get; set; }

            [JsonProperty("rgContexts")]
            public Dictionary<string, Context> RgContexts { get; set; }
        }

        public class Context
        {
            [JsonProperty("asset_count")]
            public int AssetCount { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}
