using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using Reactor.Extensions;

using UnityEngine;
using Object = UnityEngine.Object;

namespace FloofUs
{
    [HarmonyPatch]
    public static class Utils
    {
        public static PlayerControl PlayerById(byte id)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.PlayerId == id)
                {
                    return player;
                }
            }

            return null;
        }



        public static List<PlayerControl> getCrewmates(IEnumerable<GameData.PlayerInfo> infection)
        {
            var list = new List<PlayerControl>();
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                var isImpostor = false;
                foreach (var impostor in infection)
                {
                    if (player.PlayerId == impostor.Object.PlayerId)
                    {
                        isImpostor = true;
                    }
                }

                if (!isImpostor)
                {
                    list.Add(player);
                }

            }

            return list;
        }

        public static PlayerControl GetClosestPlayer(PlayerControl refplayer, List<PlayerControl> AllPlayers)
        {
            var num = double.MaxValue;
            PlayerControl result = null;
            foreach (var player in AllPlayers)
            {
                var flag3 = player.Data.IsDead;
                if (flag3) continue;
                var flag = player.PlayerId != refplayer.PlayerId;
                if (!flag) continue;
                var distBetweenPlayers = GetDistBetweenPlayers(player, refplayer);
                var flag2 = distBetweenPlayers < num;
                if (!flag2) continue;
                num = distBetweenPlayers;
                result = player;
            }

            return result;
        }

        public static PlayerControl GetClosestPlayer(PlayerControl refplayer)
        {
            return GetClosestPlayer(refplayer, PlayerControl.AllPlayerControls.ToArray().ToList());
        }


        public static double GetDistBetweenPlayers(PlayerControl player, PlayerControl refplayer)
        {
            var truePosition = refplayer.GetTruePosition();
            var truePosition2 = player.GetTruePosition();
            return Vector2.Distance(truePosition, truePosition2);
        }

        public static List<PlayerControl> getImpostors(IEnumerable<GameData.PlayerInfo> infection)
        {
            var list = new List<PlayerControl>();
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                var isImpostor = false;
                foreach (var impostor in infection)
                {
                    if (player.PlayerId == impostor.Object.PlayerId)
                    {
                        isImpostor = true;
                    }
                }

                if (isImpostor)
                {
                    list.Add(player);
                }

            }

            return list;
        }

    }

    
}