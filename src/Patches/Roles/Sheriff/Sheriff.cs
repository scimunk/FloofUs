using System;
using Essentials.Options;
using Reactor;
using UnityEngine;

namespace FloofUs
{
    public class Sheriff : RoleInfo {

        public static CustomNumberOption sheriffKillCd;
        public static CustomToggleOption sheriffKillOther;
        public static CustomToggleOption sheriffBodyReport;

        public Sheriff() : base("Sherrif", RoleTypes.Crewmate, typeof(SheriffRole)) {
            
        }

        public override void registerRole() {
            //Registerting Options
            roleChance = new CustomNumberOption("floofus.sherrif.on","<color=#FFFF00FF>Sheriff</color>", true, 0, 0, 100, 10);
            sheriffKillCd = new CustomNumberOption("floofus.sherrif.killcd","<color=#FFFF00FF>Sheriff KillCd</color>", true, 25, 10, 40, 2.5f);
            sheriffKillOther = new CustomToggleOption("floofus.sherrif.killOther", "Sheriff Miskill Kills Crewmate", true, true);
			sheriffBodyReport = new CustomToggleOption("floofus.sherrif.reportbody", "Sheriff can report who they've killed", true, false);
        }
    }

    public class SheriffRole : Role
    {

        private static Sprite Kill => FloofUsPlugin.Kill;

        public PlayerControl ClosestPlayer { get; set;}
        public DateTime LastKilled { get; set;}

        public SheriffRole(RoleInfo roleInfo, PlayerControl player) : base(roleInfo, player) 
        {
            ImpostorText = () => "Shoot the [FF0000FF]Impostor";
            TaskText = () => "Kill off the impostor but don't kill crewmates.";
            color = UnityEngine.Color.yellow;

        }

        public float SheriffKillTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastKilled;
            var num = Sheriff.sheriffKillCd.GetValue() * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }


        public override void Start(object sender, ShipStatus __instance) 
        {
            LastKilled = DateTime.UtcNow;
            LastKilled = LastKilled.AddSeconds(-8.0);
        }

    

        public override void PerformKill(object sender, KillEvent killEvent) 
        {
            if (!player.CanMove) return;
            if (player.Data.IsDead) return;

            if (SheriffKillTimer() != 0f) {
                return;
            }

            Role target = GetClosestPlayer();
            float dist = (float)GetDistanceTo(this, target);

            if (dist < (double)GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance]) 
            {
                bool hasKilled = false;
                if (target.roleInfo.type == RoleTypes.Crewmate && Sheriff.sheriffKillOther.GetValue()) {
                    hasKilled = KillManager.AttemptMurder(this, target);
                    if (hasKilled) KillManager.Suicide(this);
                }else {
                    hasKilled = KillManager.AttemptMurder(this, target);
                }
                
                killEvent.Consume(true);
                return;
            }

            
        }

        public override void HudManagerUpdate(object sender, HudManager __instance) {
            UpdateKillButton(__instance);
            //__instance.KillButton.renderer.sprite = Kill;
            if (__instance.KillButton == null) return;

            __instance.KillButton.renderer.sprite = Kill;
            bool flag = true;

            var keyInt = Input.GetKeyInt(KeyCode.Q);
            var controller = ConsoleJoystick.player.GetButtonDown(8);
            if ((keyInt | controller) && __instance.KillButton != null && flag && !PlayerControl.LocalPlayer.Data.IsDead)
            {
                __instance.KillButton.PerformKill();
            }
        }


        public void UpdateKillButton(HudManager __instance) {
            var KillButton = __instance.KillButton;
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;

             
            var isDead = PlayerControl.LocalPlayer.Data.IsDead;
            if (isDead)
            {
                KillButton.gameObject.SetActive(false);
                KillButton.isActive = false;
            }
            else
            {
                KillButton.gameObject.SetActive(!MeetingHud.Instance);
                KillButton.isActive = !MeetingHud.Instance;
                KillButton.SetCoolDown(SheriffKillTimer(), PlayerControl.GameOptions.KillCooldown + 15f);
                var closestPlayer = Utils.GetClosestPlayer(PlayerControl.LocalPlayer);
                var distBetweenPlayers = Utils.GetDistBetweenPlayers(PlayerControl.LocalPlayer, closestPlayer);
                var flag9 = distBetweenPlayers < GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
                if (flag9 && KillButton.enabled)
                {
                    KillButton.SetTarget(GetClosestPlayer().player);
                }
            }
        }

    
        
    }
}