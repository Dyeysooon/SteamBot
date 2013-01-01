using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SteamTrade
{
    public class AppKeyedCollection : KeyedCollection<int, SteamInventory.AppInventory>
    {
        protected override int GetKeyForItem (SteamInventory.AppInventory app)
        {
            return app.AppId;
        }
    }
    public class AppContextKeyedCollection : KeyedCollection<int, SteamInventory.AppContext>
    {
        protected override int GetKeyForItem (SteamInventory.AppContext appContext)
        {
            return appContext.ContextId;
        }
    }
    public class AppContextDataKeyedCollection: KeyedCollection<int, AppContextData.App>
    {
        protected override int GetKeyForItem (AppContextData.App appContextData)
        {
            return appContextData.AppId;
        }
    }
    public class SchemaKeyedCollection : KeyedCollection<int, Schema>
    {
        protected override int GetKeyForItem (Schema schema)
        {
            return schema.AppId;
        }
    }
    public class AssetPricesKeyedCollection : KeyedCollection<int, AssetPrices>
    {
        protected override int GetKeyForItem (AssetPrices asset)
        {
            return asset.AppId;
        }
    }
}
