using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
//using Il2CppSystem;
using Reactor;
using Reactor.Extensions;
using Reactor.Networking;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace FloofUs
{
    public class DeadPlayer
    {
        public byte KillerId { get; set; }
        public byte PlayerId { get; set; }
        public DateTime KillTime { get; set; }
    }

    public static class KillManager
    {


        public static bool AttemptMurder(Role killer, Role target) 
        {
            if(target.canBeKilled()) {

                RpcMurderPlayer(killer, target);

               return true;
            }

            return false;
        }

        public static void RpcMurderPlayer(Role killer, Role target) {
            MurderPlayer(killer, target);
            Rpc<CustomKillRpc>.Instance.Send(new CustomKillRpc.Data(killer, target));
        }

        public static void Suicide(Role player) {
            RpcMurderPlayer(player, player);
        }

        public static void MurderPlayer(Role killerRole, Role targetRole) {
            PlayerControl killer = killerRole.player;
            PlayerControl target = targetRole.player;
            GameData.PlayerInfo data = target.Data;
            if (data != null && !data.IsDead)
            {
                if (killer == PlayerControl.LocalPlayer)
                {
                    SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.KillSfx, false, 0.8f);
                }

                target.gameObject.layer = LayerMask.NameToLayer("Ghost");
                if (target.AmOwner)
                {
                    try
                    {
                        if (Minigame.Instance)
                        {
                            Minigame.Instance.Close();
                            Minigame.Instance.Close();
                        }

                        if (MapBehaviour.Instance)
                        {
                            MapBehaviour.Instance.Close();
                            MapBehaviour.Instance.Close();
                        }
                    }
                    catch
                    {
                    }

                    DestroyableSingleton<HudManager>.Instance.KillOverlay.ShowOne(killer.Data, data);
                    DestroyableSingleton<HudManager>.Instance.ShadowQuad.gameObject.SetActive(false);
                    target.nameText.GetComponent<MeshRenderer>().material.SetInt("_Mask", 0);
                    target.RpcSetScanner(false);
                    ImportantTextTask importantTextTask = new GameObject("_Player").AddComponent<ImportantTextTask>();
                    importantTextTask.transform.SetParent(AmongUsClient.Instance.transform, false);
                    if (!PlayerControl.GameOptions.GhostsDoTasks)
                    {
                        for (int i = 0; i < target.myTasks.Count; i++)
                        {
                            PlayerTask playerTask = target.myTasks.ToArray()[i];
                            playerTask.OnRemove();
                            UnityEngine.Object.Destroy(playerTask.gameObject);
                        }

                        target.myTasks.Clear();
                        importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                            StringNames.GhostIgnoreTasks,
                            new UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>(0));
                    }
                    else
                    {
                        importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                            StringNames.GhostDoTasks,
                            new UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>(0));
                    }

                    target.myTasks.Insert(0, importantTextTask);
                }

                killer.MyPhysics.StartCoroutine(killer.KillAnimations.Random<KillAnimation>()
                    .CoPerformKill(killer, target));
                var deadBody = new DeadPlayer
                {
                    PlayerId = target.PlayerId,
                    KillerId = killer.PlayerId,
                    KillTime = DateTime.UtcNow,
                };

                //Murder.KilledPlayers.Add(deadBody);
            }
        }

    }
}