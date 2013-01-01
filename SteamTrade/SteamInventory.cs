using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SteamTrade
{
    public class SteamInventory
    {
        // should use appid as identifier
        public AppKeyedCollection Inventories = new AppKeyedCollection();
        public AppContextData AppContextData { get; set; }
        // list of supported app ids for assetclassinfo in order tf2, tf2 beta, dota2, portal2, steam, dota2 internal test, dota2 beta test
        [JsonIgnore]
        public int[] AppIds = { 440, 520, 570, 620, 753, 816, 205790 };

        public Item GetItem(int appid, int contextid, ulong assetid)
        {
            if (Inventories[appid].AppContexts[contextid].Items.ContainsKey(assetid.ToString()))
            {
                return Inventories[appid].AppContexts[contextid].Items[assetid.ToString()];
            }
            return null;
        }

        public Item GetItem(Trade.TradeEvent tradeEvent)
        {
            if (Inventories[tradeEvent.appid].AppContexts[tradeEvent.contextid].Items.ContainsKey(tradeEvent.assetid.ToString()))
            {
                return Inventories[tradeEvent.appid].AppContexts[tradeEvent.contextid].Items[tradeEvent.assetid.ToString()];
            }
            return null;
        }

        public class AppInventory
        {
            public AppInventory()
            {
                AppId = new int();
                AppContexts = new AppContextKeyedCollection();
            }
            public int AppId { get; set; }
            
            public AppContextKeyedCollection AppContexts { get; set; }
        }

        public class AppContext
        {
            //must be manually set upon creation
            [JsonIgnore]
            public int ContextId { get; set; }

            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("rgAppInfo")]
            public AppInfo AppInfo { get; set; }

            [JsonProperty("rgInventory")]
            public Dictionary<String, Item> Items { get; set; }

            //unknown!!!
            public List<object> rgCurrency { get; set; }

            [JsonProperty("rgDescriptions")]
            public Dictionary<String, Instance> Instances { get; set; }
        }

        public class AppInfo
        {
            [JsonProperty("appid")]
            public string AppId { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("classid")]
            public string ClassId { get; set; }

            [JsonProperty("instanceid")]
            public string InstanceId { get; set; }

            [JsonProperty("amount")]
            public string Amount { get; set; }

            [JsonProperty("pos")]
            public int Position { get; set; }
        }

        public class Description
        {
            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }

            [JsonProperty("app_data")]
            public DescriptionAppData DescriptionAppData { get; set; }
        }

        public class Tag
        {
            [JsonProperty("internal_name")]
            public string InternalName { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("category")]
            public string Category { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }

            [JsonProperty("category_name")]
            public string CategoryName { get; set; }
        }

        public class DescriptionAppData
        {
            [JsonProperty("def_index")]
            public string DefIndex { get; set; }

            [JsonProperty("is_itemset_name")]
            public int IsItemsetName { get; set; }
        }

        public class AppData
        {
            [JsonProperty("slot")]
            public string Slot { get; set; }

            [JsonProperty("set_bundle_defindex")]
            public string SetBundleDefindex { get; set; }

            [JsonProperty("containing_bundles")]
            public String[] ContainingBundles { get; set; }

            [JsonProperty("workshop_contributors")]
            public List<WorkShopContributor> WorkShopContributors { get; set; }

            [JsonProperty("filter_data")]
            public Dictionary<string, Element> FilterData { get; set; }

            [JsonProperty("player_class_ids")]
            public string[] PlayerClassIds { get; set; }

            [JsonProperty("highlight_color")]
            public string HighlightColor { get; set; }

            [JsonProperty("quantity")]
            public string Quantity { get; set; }

            [JsonProperty("def_index")]
            public string Defindex { get; set; }

            [JsonProperty("quality")]
            public string Quality { get; set; }

        }

        public class WorkShopContributor
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Element
        {
            [JsonProperty("element_ids")]
            public string[] ElementIds { get; set; }
        }

        public class Instance
        {
            [JsonProperty("appid")]
            public string AppId { get; set; }

            [JsonProperty("classid")]
            public string ClassId { get; set; }

            [JsonProperty("instanceid")]
            public string InstanceId { get; set; }

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

            [JsonProperty("tags")]
            public List<Tag> Tags { get; set; }

            [JsonProperty("app_data")]
            public AppData AppData { get; set; }
        }
    }

    
}
