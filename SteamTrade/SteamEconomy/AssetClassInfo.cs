using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamTrade
{
    public class AssetClassInfo
    {
        public static Result FetchAssetClassInfo(int appid, string apiKey, List<string> classIds)
        {
            StringBuilder sb = new StringBuilder();
            string c = "classid";
            for (int i=0; i< classIds.Count; i++)
            {
                sb.Append("&classid" + i + "=" + classIds[i]);
            }
            string url = String.Format (
                "http://api.steampowered.com/ISteamEconomy/GetAssetClassInfo/v0001/?key=" + apiKey + "&appid=" + appid + "&class_count=" + classIds.Count + sb.ToString()
            );
            
            try
            {
                string response = SteamWeb.Fetch (url, "GET", null, null, true);
                System.IO.File.WriteAllText("assetclassinfo.inventory", response);
                Result r = JsonConvert.DeserializeObject<Result>(response);
                System.IO.File.WriteAllText("dassetclassinfodeser.inventory", JsonConvert.SerializeObject(r,Formatting.Indented));
                return JsonConvert.DeserializeObject<Result>(response);
            }
            catch (Exception)
            {
                //return JsonConvert.DeserializeObject ("{\"success\":\"false\"}");
            }
            return null;
        }
        public class Result
        {
            public bool success { get; set; }
            public List<Item> items { get; set; }
        }
        public class Item
        {
            [JsonProperty("icon_url")]
            public string IconUrl { get; set; }

            [JsonProperty("icon_url_large")]
            public string IconUrlLarge { get; set; }

            [JsonProperty("icon_drag_url")]
            public string IconDragUrl { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("market_name")]
            public string MarketName { get; set; }

            [JsonProperty("name_color")]
            public string NameColor { get; set; }

            [JsonProperty("background_color")]
            public string BackgroundColor { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("tradable")]
            public int Tradable { get; set; }

            [JsonProperty("marketable")]
            public int Marketable { get; set; }

            [JsonProperty("fraudwarnings")]
            public List<string> FraudWarnings { get; set; }

            [JsonProperty("descriptions")]
            public List<Description> Descriptions { get; set; }

            [JsonProperty("actions")]
            public List<Action> Actions { get; set; }

            [JsonProperty("owner_actions")]
            public List<Action> OwnerActions { get; set; }

            [JsonProperty("tags")]
            public List<Tag> Tags { get; set; }

            [JsonProperty("classid")]
            public string ClassId { get; set; }
        }

        public class Description
        {
            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }
        }
        public class Action
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }
        }
        public class Tag
        {
            [JsonProperty("internal_name")]
            public string InternalName { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("category_name")]
            public string CategoryName { get; set; }

            [JsonProperty("category")]
            public string Category { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }
        }
    }
}
