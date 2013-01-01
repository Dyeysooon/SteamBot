using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SteamTrade
{
    public class AssetPrices
    {
        public static int[] ValidAppIDs = { 440, 520, 570, 620, 816, 205790 };

        public static AssetPricesKeyedCollection FetchAllAssetPrices(string apiKey, string language = "")
        {
            AssetPricesKeyedCollection assetPrices = new AssetPricesKeyedCollection();
            foreach (int id in ValidAppIDs)
            {
                assetPrices.Add(FetchAssetPrices(id, apiKey, language));
            }
            return assetPrices;
        }

        public static AssetPrices FetchAssetPrices(int appid, string apiKey, string language = "")
        {
            if (!ValidAppIDs.Contains(appid))
                throw new ArgumentOutOfRangeException("see http://wiki.teamfortress.com/wiki/WebAPI#appids for list of valid ids");
            if (language != null)
            {
                language = "&language=" + language;
            }
            string url = String.Format("http://api.steampowered.com/ISteamEconomy/GetAssetPrices/v0001/?key={0}&appid={1}{2}", apiKey, appid, language);
            Console.WriteLine("Fetching AssetPrices for appid:" + appid + " from " + url);

            try
            {
                string response = SteamWeb.Fetch(url, "GET", null, null, true);
                System.IO.File.WriteAllText("assetprices_" + appid + ".prices", response);
                AssetPrices assetPrices = JsonConvert.DeserializeObject<AssetPrices>(response);
                assetPrices.AppId = appid;
                assetPrices.classids = new Dictionary<string, int>();
                foreach (var asset in assetPrices.Result.Assets)
                {
                    foreach (var assetClass in asset.Class)
                    {
                        if (assetClass.Name == "def_index")
                        {
                            assetPrices.classids.Add(asset.ClassId, Convert.ToInt32(assetClass.Value));
                        }
                    }
                }
                Console.WriteLine("assets " + assetPrices.Result.Assets.Count + " in: " + appid);
                System.IO.File.WriteAllText("defindexes_" + appid+ ".json", JsonConvert.SerializeObject(assetPrices.classids));
                System.IO.File.WriteAllText("assetprices2_" + appid + ".prices", JsonConvert.SerializeObject(assetPrices, Formatting.Indented));
                return assetPrices;
            }
            catch (Exception)
            {
                //return JsonConvert.DeserializeObject ("{\"success\":\"false\"}");
            }
            return null;
        }

        public bool GetDefindexForClassId(string classId, out int defindex)
        {
            if (classids.ContainsKey(classId))
            {
                defindex = classids[classId];
                return true;
            }
            // this should only happen if the schema is not up to date
            defindex = -1;
            return false;
        }
        //[JsonIgnore]
        public Dictionary<string, int> classids { get; set; }

        [JsonIgnore]
        public int AppId { get; set; }

        [JsonProperty("result")]
        public ResponseResult Result { get; set; }


        public class ResponseResult
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("assets")]
            public List<Asset> Assets { get; set; }

            [JsonProperty("tags")]
            public dynamic Tags { get; set; }

            [JsonProperty("tag_ids")]
            public dynamic TagIds { get; set; }
        }
            
        public class Asset
        {
            [JsonProperty("prices")]
            public Currencies Prices { get; set; }

            [JsonProperty("original_prices")]
            public Currencies OriginalPrices { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("date")]
            public string Date { get; set; }

            [JsonProperty("class")]
            public List<AssetClass> Class { get; set; }

            [JsonProperty("classid")]
            public string ClassId { get; set; }

            [JsonProperty("tags")]
            public List<string> Tags { get; set; }

            //appid 816 differs from the rest!
            [JsonProperty("tag_ids")]
            public dynamic[] TagIds { get; set; }
            
        }

        // will need to add extra if it changes in the future 
        public class Currencies
        {
            [JsonProperty("USD")]
            public int USD { get; set; }

            [JsonProperty("GBP")]
            public int GBP { get; set; }

            [JsonProperty("EUR")]
            public int EUR { get; set; }

            [JsonProperty("RUB")]
            public int RUB { get; set; }

            [JsonProperty("BRL")]
            public int BRL { get; set; }
        }

        public class AssetClass
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}
