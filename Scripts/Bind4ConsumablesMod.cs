using PugMod;
using System.Linq;
using UnityEngine;


namespace B4C
{
    public class Bind4Consumables : IMod
    {
        public const string VERSION = "0.2.1";
        public const string NAME = "Bind4Consumables";
        public const string Author = "gdel35";

        public static AssetBundle assetBundle;
        private LoadedMod modInfo;
        public static LoadedMod GetModInfo(IMod mod)
        {
            return API.ModLoader.LoadedMods.FirstOrDefault(modInfo => modInfo.Handlers.Contains(mod));
        }

        public void EarlyInit()
        {
            UnityEngine.Debug.Log($"[{NAME}]: Mod version: {VERSION}");
            modInfo = GetModInfo(this);

            if (modInfo != null)
            {
                assetBundle = modInfo.AssetBundles[0];
            }
            else
            {
                UnityEngine.Debug.LogError($"Could not find mod info for [{NAME}].");
            }
            UnityEngine.Debug.Log($"[{NAME}]: Mod loaded successfully");
        }

        public void Init()
        {
        }

        public void ModObjectLoaded(Object obj)
        {
        }

        public void Shutdown()
        {
        }

        public void Update()
        {
        }
    }

}
