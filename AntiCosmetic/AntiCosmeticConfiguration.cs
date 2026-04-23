using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Foogle.AntiCosmetic
{
    public class AntiCosmeticConfiguration : IRocketPluginConfiguration
    {
        public bool RemoveHat;
        public bool RemoveGlasses;
        public bool RemoveMask;
        public bool RemoveShirt;
        public bool RemovePants;
        public bool RemoveVest;
        public bool RemoveBackpack;

        public float CheckIntervalSeconds;

        [XmlArrayItem("SteamId")]
        public List<string> BypassIds;

        public void LoadDefaults()
        {
            RemoveHat      = true;
            RemoveGlasses  = true;
            RemoveMask     = true;
            RemoveShirt    = true;
            RemovePants    = true;
            RemoveVest     = true;
            RemoveBackpack = true;

            CheckIntervalSeconds = 5f;

            BypassIds = new List<string>
            {
                
            };
        }
    }
}