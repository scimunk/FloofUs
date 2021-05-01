using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Hazel;
//using Il2CppSystem;
using Reactor;
using Reactor.Extensions;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;



namespace FloofUs
{

    public static class RpcHandling
    {

        //Setting Roles Here
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
        public static class RpcSetInfected
        {
            public static void Prefix([HarmonyArgument(0)] Il2CppReferenceArray<GameData.PlayerInfo> infected)
            {

                RolesManager.DispatchRole(infected);

            }
        }

    }
}