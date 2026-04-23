using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections;
using System.Reflection;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Foogle.AntiCosmetic
{
    public class AntiCosmeticPlugin : RocketPlugin<AntiCosmeticConfiguration>
    {
        public static AntiCosmeticPlugin Instance { get; private set; }
        private Coroutine _loop;

        protected override void Load()
        {
            Instance = this;
            Logger.Log($"{Name} {Assembly.GetName().Version.ToString(3)} has been loaded!");
            _loop = StartCoroutine(StripLoop());
        }

        protected override void Unload()
        {
            Logger.Log($"{Name} has been unloaded!");
            if (_loop != null)
                StopCoroutine(_loop);
        }

        private static void StripPlayer(UnturnedPlayer player)
        {
            AntiCosmeticConfiguration cfg = Instance.Configuration.Instance;
            PlayerClothing c = player.Player.clothing;

            if (cfg.BypassIds.Contains(player.CSteamID.m_SteamID.ToString()))
                return;

            c.ServerSetVisualToggleState(EVisualToggleType.COSMETIC, false);
            c.ServerSetVisualToggleState(EVisualToggleType.SKIN, false);
            c.ServerSetVisualToggleState(EVisualToggleType.MYTHIC, false);

            HumanClothes tc = c.thirdClothes;
            if (tc == null) return;

            if (cfg.RemoveHat)      tc.visualHat      = 0;
            if (cfg.RemoveGlasses)  tc.visualGlasses  = 0;
            if (cfg.RemoveMask)     tc.visualMask     = 0;
            if (cfg.RemoveShirt)    tc.visualShirt    = 0;
            if (cfg.RemovePants)    tc.visualPants    = 0;
            if (cfg.RemoveVest)     tc.visualVest     = 0;
            if (cfg.RemoveBackpack) tc.visualBackpack = 0;

            tc.apply();
        }

        private IEnumerator StripLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(Instance.Configuration.Instance.CheckIntervalSeconds);

                foreach (SteamPlayer steamPlayer in Provider.clients)
                {
                    UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(steamPlayer);
                    if (player == null) continue;
                    StripPlayer(player);
                }
            }
        }
    }
}