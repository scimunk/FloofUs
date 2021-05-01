using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
//using Il2CppSystem;
using Reactor;
using Reactor.Extensions;
using Essentials.Options;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace FloofUs
{
    public enum RoleTypes : uint
    {
        Universal,
        Crewmate,
        Neutral,
        Impostor,
    }

    public abstract class RoleInfo
    {
        public string name = "Test";
        public RoleTypes type;
        public Type roleClass;

        public static byte IdPool = 0;

        public byte id;

        public CustomNumberOption roleChance;

        public RoleInfo(string _name, RoleTypes _type, Type _roleClass) {
            name = _name;
            type = _type;
            roleClass = _roleClass;

            id = IdPool++;
        }


        

        public abstract void registerRole();
        

    }
}