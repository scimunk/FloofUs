using System;
using Essentials.Options;
using Reactor;
using UnityEngine;

namespace FloofUs
{
    public class NoRoleInfo : RoleInfo {

        public NoRoleInfo() : base("NoRole", RoleTypes.Universal, typeof(NoRole)) {
            
        }

        public override void registerRole() {

        }
    }

    public class NoRole : Role
    {
        public NoRole(RoleInfo roleInfo, PlayerControl player) : base(roleInfo, player) 
        {

        
        }
    
    }
}