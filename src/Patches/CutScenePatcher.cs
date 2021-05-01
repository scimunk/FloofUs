using HarmonyLib;
using Reactor;

namespace FloofUs
{

    [HarmonyPatch]
    public static class IntroCutScenePatch
    {

        [HarmonyPatch(typeof(IntroCutscene.Nested_0), nameof(IntroCutscene.Nested_0.MoveNext))]
        public static class IntroCutsceneLiveBext
        {
            public static void Postfix(IntroCutscene.Nested_0 __instance)
            {
                Role role = RolesManager.GetPlayerRole(PlayerControl.LocalPlayer);

                PluginSingleton<FloofUsPlugin>.Instance.Log.LogInfo(string.Format("Displaying Player Role {0} with Text {1}", role.name, role.ImpostorText.Invoke()));

                var alpha = __instance.__this.Title.color.a;
                if (role != null && !role.IsRoleHidden())
                {

                    __instance.__this.Title.text = role.name;
                    __instance.__this.Title.color = role.color;
                    __instance.__this.ImpostorText.text = role.ImpostorText.Invoke();
                    __instance.__this.ImpostorText.gameObject.SetActive(true);
                    __instance.__this.BackgroundBar.material.color = role.color;

                }
            }
        }

    }
}