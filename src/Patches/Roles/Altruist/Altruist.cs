using System;
//using FloofUs.CustomOption;
using Essentials.Options;
using UnityEngine;

namespace FloofUs
{
    public class Altruist : RoleInfo {
        public static CustomNumberOption altruistOn;
        public static CustomNumberOption reviveDurationOption;
        public static CustomToggleOption targetBodyOption;

        public Altruist() : base("Altruist", RoleTypes.Crewmate, typeof(AltruistRole)) {

        }

        public override void registerRole() {
            //Registerting Options
            roleChance = new CustomNumberOption("floofus.altruist.on","<color=#660000FF>Altruist</color>", true, 0, 0, 100, 10f);
            reviveDurationOption = new CustomNumberOption("floofus.altruist.reviveDuration","<color=#660000FF>revive Duration</color>", true, 10, 0, 20, 1f);
            targetBodyOption = new CustomToggleOption("floofus.altruist.targetBody","<color=#660000FF>Target Body</color>", true, true);

            reviveDurationOption.HudVisible = false;
            targetBodyOption.HudVisible = false;
        }
    }

    public class AltruistRole : Role
    {

     
        public PlayerControl ClosestPlayer { get; set;}
        public DateTime LastKilled { get; set;}

        public AltruistRole(RoleInfo roleInfo, PlayerControl player) : base(roleInfo, player) 
        {
            ImpostorText = () => "Sacrifice yourself to save another";
            TaskText = () => "Revive a dead body at the cost of your own life.";
            color = new Color(0.4f, 0f, 0f, 1f);
        }
       


        
 
        
        
        
    }
}