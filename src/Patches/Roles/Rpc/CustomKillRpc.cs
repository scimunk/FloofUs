using System;
using Reactor.Networking;
using Reactor;
using FloofUs;
using Hazel;

namespace FloofUs
{
    [RegisterCustomRpc((uint) CustomRpcCalls.CustomKill)]
    public class CustomKillRpc : PlayerCustomRpc<FloofUsPlugin, CustomKillRpc.Data>
    {

        public CustomKillRpc(FloofUsPlugin plugin, uint id) : base(plugin, id){
            
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;

        public readonly struct Data
        {   
            public readonly Role Killer;
            public readonly Role Target;

            public Data(Role killer, Role target)
            {
                Killer = killer;
                Target = target;
            }
        }

        public override void Handle(PlayerControl innerNetObject, Data data) {
            PluginSingleton<FloofUsPlugin>.Instance.Log.LogInfo("Handling CustomKill Rpc");
            KillManager.MurderPlayer(data.Killer, data.Target);
        }


        public override Data Read(MessageReader reader) 
        {
            var killer = RolesManager.GetPlayerRole(reader.ReadByte());
            var target = RolesManager.GetPlayerRole(reader.ReadByte());
            return new Data(killer, target);
        }

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.Killer.player.PlayerId);
            writer.Write(data.Target.player.PlayerId);
        }
        

    }
}