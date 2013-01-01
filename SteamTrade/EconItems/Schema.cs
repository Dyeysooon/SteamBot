using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace SteamTrade
{
    public class Schema
    {
        public static int[] ValidAppIDs = { 440, 520, 570, 620, 816, 205790 };

        public static Schema FetchSchema(string apiKey, int appid, string language = "")
        {
            //string[] validLanguages = {"da", "da_DK", "nl", "nl_NL", "en", "en_US", "fi"};
            if (!ValidAppIDs.Contains(appid))
                throw new ArgumentOutOfRangeException("see http://wiki.teamfortress.com/wiki/WebAPI#appids for list of valid ids");
            if (language != null)
            {
                language = "&language=" + language;
            }
            var url = String.Format("http://api.steampowered.com/IEconItems_{0}/GetSchema/v0001/?key={1}{2}", appid, apiKey, language);
            Console.WriteLine("Fetching Schema for appid:" + appid + " from " + url);
            if (!Directory.Exists("cache"))
            {
                Directory.CreateDirectory("cache");
            }
            string cachefile = Path.Combine("cache","schema_" + appid + ".cache");
            var result = "";
            bool updated = false;
            HttpWebResponse response = SteamWeb.Request(url, "GET");

            DateTime schemaLastModified = DateTime.Parse(response.Headers["Last-Modified"]);

            if (!System.IO.File.Exists(cachefile) || (schemaLastModified > System.IO.File.GetCreationTime(cachefile)))
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
                File.WriteAllText(cachefile, result);
                System.IO.File.SetCreationTime(cachefile, schemaLastModified);
                updated = true;
            }
            else
            {
                TextReader reader = new StreamReader(cachefile);
                result = reader.ReadToEnd();
                reader.Close();
            }
            response.Close();

            SchemaResult schemaResult = JsonConvert.DeserializeObject<SchemaResult>(result);
            schemaResult.Result.Updated = updated;
            schemaResult.Result.AppId = appid;
            File.WriteAllText("schema_" + appid + ".json", JsonConvert.SerializeObject(schemaResult, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            Console.WriteLine("schmea " + schemaResult.Result.Items.Count + " in :" + appid);
            return schemaResult.Result ?? null;
        }
        public static SchemaKeyedCollection FetchAllSchemas (string apiKey, int[] appIds, string language = null)
        {
            var schemas = new SchemaKeyedCollection();
            //List<Schema> schemas = new List<Schema>();
            foreach (var appid in appIds)
            {
                schemas.Add(FetchSchema(apiKey, appid, language));
            }
            return schemas;
        }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("items_game_url")]
        public string ItemsGameUrl { get; set; }

        [JsonProperty("qualities")]
        public Dictionary<string, int> Qualities { get; set; }

        [JsonProperty("qualityNames")]
        public Dictionary<string, string> QualityNames { get; set; }

        [JsonProperty("originNames")]
        public List<OriginName> OriginNames { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }

        [JsonProperty("item_sets")]
        public List<ItemSet> ItemSets { get; set; }

        [JsonProperty("attribute_controlled_attached_particles")]
        public List<Particles> AttributeControlledAttachedParticles { get; set; }

        [JsonProperty("item_levels")]
        public List<ItemLevel> ItemLevels { get; set; }

        [JsonProperty("kill_eater_ranks")]
        public List<KillEaterRank> KillEaterRanks { get; set; }

        [JsonProperty("kill_eater_score_types")]
        public List<KillEaterScoreType> KillEaterScoreTypes { get; set; }

        [JsonProperty("string_lookups")]
        public List<StringLookup> StringLookups { get; set; }

        [JsonIgnore]
        public bool Updated = false;

        [JsonIgnore]
        public int AppId { get; set; }

        public class Attribute
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("defindex")]
            public int Defindex { get; set; }

            [JsonProperty("attribute_class")]
            public string AttributeClass { get; set; }

            [JsonProperty("minvalue")]
            public float MinValue { get; set; }

            [JsonProperty("maxvalue")]
            public float MaxValue { get; set; }

            [JsonProperty("description_string")]
            public string DescriptionString { get; set; }

            [JsonProperty("description_format")]
            public string DescriptionFormat { get; set; }

            [JsonProperty("effect_type")]
            public string EffectType { get; set; }

            [JsonProperty("hidden")]
            public bool Hidden { get; set; }

            [JsonProperty("stored_as_integer")]
            public bool StoredAsInteger { get; set; }
        }

        public class ItemAttribute
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("class")]
            public string Class { get; set; }

            [JsonProperty("value")]
            public double Value { get; set; }
        }

        public class ItemLevel
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("levels")]
            public List<Level> Levels { get; set; }
        }

        public class ItemSet
        {
            [JsonProperty("item_set")]
            public string Item_Set { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("store_bundle")]
            public string StoreBundle { get; set; }

            [JsonProperty("items")]
            public List<string> Items { get; set; }

            [JsonProperty("attributes")]
            public List<ItemAttribute> Attributes { get; set; }
        }

        public class ItemStyle
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class ItemTool
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("usage")]
            public Dictionary<string, string> Usage { get; set; }

            [JsonProperty("use_string")]
            public string UseString { get; set; }

            [JsonProperty("usage_capabilities")]
            public Dictionary<string, bool> UsageCapabilities { get; set; }

            [JsonProperty("restriction")]
            public string Restriction { get; set; }
        }

        public class KillEaterRank
        {
            [JsonProperty("level")]
            public int RankLevel { get; set; }

            [JsonProperty("required_score")]
            public int RequiredScore { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class KillEaterScoreType
        {
            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("type_name")]
            public string TypeName { get; set; }

            [JsonProperty("level_data")]
            public string LevelData { get; set; }
        }

        public class Level
        {
            [JsonProperty("level")]
            public int level { get; set; }

            [JsonProperty("required_score")]
            public int RequiredScore { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class OriginName
        {
            [JsonProperty("origin")]
            public int Origin { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Particles
        {
            [JsonProperty("system")]
            public string System { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("attach_to_rootbone")]
            public bool AttachToRootbone { get; set; }

            [JsonProperty("attachment")]
            public string Attachment { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Item
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("defindex")]
            public int Defindex { get; set; }

            [JsonProperty("item_class")]
            public string ItemClass { get; set; }

            [JsonProperty("item_type_name")]
            public string ItemTypeName { get; set; }

            [JsonProperty("item_name")]
            public string ItemName { get; set; }

            [JsonProperty("item_description")]
            public string ItemDescription { get; set; }

            [JsonProperty("proper_name")]
            public bool ProperName { get; set; }

            [JsonProperty("item_slot")]
            public string ItemSlot { get; set; }

            [JsonProperty("model_player")]
            public string ModelPlayer { get; set; }

            [JsonProperty("item_quality")]
            public int ItemQuality { get; set; }

            [JsonProperty("image_inventory")]
            public string ImageInventory { get; set; }

            [JsonProperty("min_ilevel")]
            public int MinIlevel { get; set; }

            [JsonProperty("max_ilevel")]
            public int MaxIlevel { get; set; }

            [JsonProperty("image_url")]
            public string ImageUrl { get; set; }

            [JsonProperty("image_url_large")]
            public string ImageUrlLarge { get; set; }

            [JsonProperty("drop_type")]
            public string DropType { get; set; }

            [JsonProperty("item_set")]
            public string ItemSet { get; set; }

            [JsonProperty("holiday_restriction")]
            public string HolidayRestriction { get; set; }

            [JsonProperty("craft_class")]
            public string CraftClass { get; set; }

            [JsonProperty("craft_material_type")]
            public string CraftMaterialType { get; set; }

            [JsonProperty("capabilities")]
            public Dictionary<string, bool> Capabilities { get; set; }

            [JsonProperty("tool")]
            public ItemTool Tool { get; set; }

            [JsonProperty("used_by_classes")]
            public string[] UsedByClasses { get; set; }

            [JsonProperty("per_class_loadout_slots")]
            public Dictionary<string, string> PerClassLoadoutSlots { get; set; }

            [JsonProperty("styles")]
            public List<ItemStyle> Styles { get; set; }

            [JsonProperty("attributes")]
            public List<ItemAttribute> Attributes { get; set; }
        }

        public class SchemaResult
        {
            [JsonProperty("result")]
            public Schema Result { get; set; }
        }

        public class StringAttribute
        {
            [JsonProperty("index")]
            public int Index { get; set; }

            [JsonProperty("string")]
            public string StringName { get; set; }
        }

        public class StringLookup
        {
            [JsonProperty("table_name")]
            public string TableName { get; set; }

            [JsonProperty("strings")]
            public StringAttribute[] Strings { get; set; }
        }

        /// <summary>
        /// Find an Item by it's defindex.
        /// </summary>
        public Item GetItem(int defindex)
        {
            foreach (Item item in Items)
            {
                if (item.Defindex == defindex)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// Returns all Items of the given crafting material.
        /// </summary>
        /// <param name="material">Item's craft_material_type JSON property.</param>
        // /// <seealso cref="Item"/>
        public List<Item> GetItemsByCraftingMaterial(string material)
        {
            return Items.Where(item => item.CraftMaterialType == material).ToList();
        }
    }
}

