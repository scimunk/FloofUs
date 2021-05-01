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


    public abstract class Buff
    {
        public string name;
    
        public Color Color;


        public Buff(Role role) {
         
        }

    }
}