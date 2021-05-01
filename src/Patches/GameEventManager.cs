using System.Linq;
using HarmonyLib;
using UnityEngine;
using System;

namespace FloofUs
{
    public class KillEvent {
        public bool consume = false;
        KillButtonManager buttonManager;

        public KillEvent(KillButtonManager _buttonManager) {
            buttonManager = _buttonManager;
        }

        public bool Consume(bool _consume) 
        {
            consume = _consume;
            return consume;
        }
    }

    public class EndGameEvent {
        ShipStatus __instance;
        GameOverReason reason;

        public EndGameEvent( ShipStatus ___instance, GameOverReason _reason) {
            __instance = ___instance;
            reason = _reason;
        }

    }

    public class GameEventManager {
        public static event EventHandler<HudManager> HudManagerUpdate;
        public static event EventHandler<ShipStatus> Start;
        public static event EventHandler<KillEvent> PerformKill;
        public static event EventHandler<EndGameEvent> EndGame;

        public static void callHudManagerUpdate(HudManager __instance) { HudManagerUpdate?.Invoke(null, __instance); }
        public static void callStart(ShipStatus __instance) { Start?.Invoke(null, __instance); }
        public static void callPerformKill(KillEvent __event) { PerformKill?.Invoke(null, __event); }
        public static void callEndGame(EndGameEvent endEvent) { EndGame?.Invoke(null, endEvent); }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class KillButtonSprite
    {        

        public static void Postfix(HudManager __instance)
        {
            GameEventManager.callHudManagerUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        
        public static void Postfix(ShipStatus __instance)
        {
             GameEventManager.callStart(__instance);        
        }
    }

      
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public static class Kill
    {

        [HarmonyPriority(Priority.First)]
        private static bool Prefix(KillButtonManager __instance)
        {
            KillEvent killEvent = new KillEvent(__instance);
            GameEventManager.callPerformKill(killEvent);        
           
            return killEvent.consume;
        }
    }

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.RpcEndGame))]
    public class EndGame
    {
        public static bool Prefix(ShipStatus __instance, [HarmonyArgument(0)] GameOverReason reason)
        {
            EndGameEvent endEvent = new EndGameEvent(__instance, reason);
            GameEventManager.callEndGame(endEvent);        

            return true;
        }
    }

}