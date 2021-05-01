using HarmonyLib;
using Reactor;

namespace FloofUs
{

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    [HarmonyPriority(Priority.Last)]
    public static class EndGameManagerPatch {
        public static bool Prefix(EndGameManager __instance) {
            PluginSingleton<FloofUsPlugin>.Instance.Log.LogInfo("Cleaning Up Game");
            RolesManager.CleanUp();
            return true;
        }
    }
}
