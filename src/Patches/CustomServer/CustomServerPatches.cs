using HarmonyLib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnhollowerBaseLib;

namespace FloofUs.CustomServer
{
    public class CustomServerInfo
    {
        public string name;
        public string ip;
        public int port;

        public CustomServerInfo(string name, string ip, int port)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
        }

        public override string ToString()
        {
            return $"{name} ({ip}:{port})";
        }
    }

    public static class CustomServersPatches
    {
        public static List<CustomServerInfo> customServers = new List<CustomServerInfo>();

        private static IRegionInfo[] _defaultRegions = ServerManager.DefaultRegions;
        

        [HarmonyPatch(typeof(RegionMenu), nameof(RegionMenu.OnEnable))]
        public static class RegionMenuOnEnablePatch
        {
            public static bool forceReloadServers;

            public static bool Prefix(ref RegionMenu __instance)
            {
                ClearOnClickAction(__instance.ButtonPool);

                CustomServerInfo customServer = new CustomServerInfo("FloofUs", "77.55.217.159", 22023);

                customServers.Add(customServer);

                if (ServerManager.DefaultRegions.Count != 3 + customServers.Count || forceReloadServers)
                {
                    var regions = new IRegionInfo[3 + customServers.Count];

  

                    for (int i = 0; i < 3; i++)
                    {
                        regions[i] = _defaultRegions[i];
                    }

                    for (int i = 0; i < customServers.Count; i++)
                    {
                        Il2CppReferenceArray<ServerInfo> servers = new [] { new ServerInfo(customServers[i].name, customServers[i].ip, (ushort)customServers[i].port) };

                        regions[i + 3] = new DnsRegionInfo(customServers[i].ip, customServers[i].name, StringNames.NoTranslation, servers).Cast<IRegionInfo>();
                    }

                    ServerManager.DefaultRegions = regions;
                    ServerManager.Instance.AvailableRegions = regions;
                    ServerManager.Instance.SaveServers();
                }

                return true;
            }

            public static void ClearOnClickAction(ObjectPoolBehavior buttonPool)
            {
                foreach (var button in buttonPool.activeChildren)
                {
                    var buttonComponent = button.GetComponent<PassiveButton>();
                    if (buttonComponent != null)
                        buttonComponent.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                }

                foreach (var button in buttonPool.inactiveChildren)
                {
                    var buttonComponent = button.GetComponent<PassiveButton>();
                    if (buttonComponent != null)
                        buttonComponent.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                }
            }
        }

        
    }
}