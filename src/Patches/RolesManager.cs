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
    public static class RolesManager
    {
        private static RoleInfo noRole;
        private static readonly List<RoleInfo> RoleTypeList = new List<RoleInfo>();

        private static Dictionary<byte, Role> PlayerToRoleDict = new Dictionary<byte, Role>();

        public static void InitializeRoles() {
            noRole = new NoRoleInfo();
            registerRole(noRole);
            registerRole(new Sheriff());
            registerRole(new Altruist());
        }

        public static RoleInfo registerRole(RoleInfo roleType) {
                roleType.registerRole();
                RoleTypeList.Add(roleType);
                return roleType;
        }

        public static void DispatchRole(Il2CppReferenceArray<GameData.PlayerInfo> infected) {
            CleanUp();

            var crewmatesRole = GetRolesByType(RoleTypes.Crewmate);
            
            var crewmates = Utils.getCrewmates(infected);
            var impostors = Utils.getImpostors(infected);

            for (int i = crewmatesRole.Count - 1; i >= 0; i--)
            {
                RoleInfo role = crewmatesRole[i];

                if (crewmates.Count <= 0) break;
                var rand = UnityEngine.Random.RandomRangeInt(0, crewmates.Count); //TODO - change
                var player = crewmates[rand];

                AssignRole(role, player);
                Rpc<SetRoleRpc>.Instance.Send(new SetRoleRpc.Data(role, player));

                crewmates.Remove(player);

                crewmatesRole.RemoveAt(i);
            }

            

            foreach(PlayerControl player in crewmates) {
                AssignRole(noRole, player);
                Rpc<SetRoleRpc>.Instance.Send(new SetRoleRpc.Data(noRole, player));
            }

             foreach(PlayerControl player in impostors) {
                AssignRole(noRole, player);
                Rpc<SetRoleRpc>.Instance.Send(new SetRoleRpc.Data(noRole, player));
            }

            PluginSingleton<FloofUsPlugin>.Instance.Log.LogInfo("dispatching roles!" + crewmatesRole.Count.ToString());
        }

        //Called by everyone uppon reception of the AssignRole Rpc (or by host)
        public static void AssignRole(RoleInfo roleInfo, PlayerControl player){
            PluginSingleton<FloofUsPlugin>.Instance.Log.LogInfo("Assigning Roles" + roleInfo.name + " To " + player.PlayerId.ToString());
            Role role = (Role)Activator.CreateInstance(roleInfo.roleClass, new object[] {roleInfo, player});
            PlayerToRoleDict[player.PlayerId] = role;
        }

        public static Role GetPlayerRole(byte playerId) {
            return PlayerToRoleDict[playerId];
        }

        public static Role GetPlayerRole(PlayerControl player) {
            return PlayerToRoleDict[player.PlayerId];
        }

        public static void CleanUp() {
            foreach(var role in PlayerToRoleDict.Values)
            {
                role.CleanUp();
            }
            PlayerToRoleDict.Clear();
        }

        public static List<RoleInfo> GetRolesByType(RoleTypes type) {
            List<RoleInfo> roles = new List<RoleInfo>();
            foreach (RoleInfo role in RoleTypeList) {
                if (role.type == type) {
                    roles.Add(role);
                }
            }
            return  roles;
        }

        public static RoleInfo GetRoleInfoById(byte id) {
            foreach (RoleInfo role in RoleTypeList) {
                if (role.id == id) {
                    return role;
                }
            }
            return null;
        }

    }
}