using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
//using Il2CppSystem;
using Reactor;
using Reactor.Extensions;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace FloofUs
{


    public abstract class Role
    {
        public PlayerControl player;
        public RoleInfo roleInfo;
        public string name;
        public Func<string> ImpostorText;
        public Func<string> TaskText;

        public float Scale { get; set; } = 1f;

        public Color color;


        public Role(RoleInfo _roleInfo, PlayerControl _player) {
            player = _player;
            name = _roleInfo.name;
            roleInfo = _roleInfo;

            ImpostorText = () => "";
            TaskText = () => "";

            
            if (PlayerControl.LocalPlayer == player) 
            {
                GameEventManager.HudManagerUpdate += HudManagerUpdate;
                GameEventManager.Start += Start;
                GameEventManager.PerformKill += PerformKill;
                GameEventManager.EndGame += EndGame;
            }
        }

        public void CleanUp() {
            if (PlayerControl.LocalPlayer == player) 
            {
                GameEventManager.HudManagerUpdate -= HudManagerUpdate;
                GameEventManager.Start -= Start;
                GameEventManager.PerformKill -= PerformKill;
                GameEventManager.EndGame -= EndGame;
            }
        }

        ~Role() {
            CleanUp();
        }


        public virtual void PerformKill(object sender, KillEvent killEvent) { }
        public virtual void Start(object sender, ShipStatus __instance) { }
        public virtual void EndGame(object sender, EndGameEvent endEvent) { }
        public virtual void HudManagerUpdate(object sender, HudManager __instance) { }
        

        public Role GetClosestPlayer() {
            var closestPlayer = Utils.GetClosestPlayer(player);
            return RolesManager.GetPlayerRole(closestPlayer);
        }

        public static double GetDistanceTo(Role player, Role refplayer)
        {
            var truePosition = player.player.GetTruePosition();
            var truePosition2 = refplayer.player.GetTruePosition();
            return Vector2.Distance(truePosition, truePosition2);
        }

        //If the role shouldn't be displayed to the user
        public bool IsRoleHidden() {
            return false;
        }

        public bool canBeKilled() {
            return true;
        }

        public static bool canVent() {
            return false;
        }


    }
}