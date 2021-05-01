using System;
using Reactor.Networking;
using Reactor;
using FloofUs;
using Hazel;

namespace FloofUs
{
    [RegisterCustomRpc((uint) CustomRpcCalls.SetRole)]
    public class SetRoleRpc : PlayerCustomRpc<FloofUsPlugin, SetRoleRpc.Data>
    {

        public SetRoleRpc(FloofUsPlugin plugin, uint id) : base(plugin, id){
            
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;

        public readonly struct Data
        {   
            public readonly PlayerControl Player;
            public readonly RoleInfo RoleInfo;

            public Data(RoleInfo roleInfo, PlayerControl player)
            {
                Player = player;
                RoleInfo = roleInfo;
            }
        }

        public override void Handle(PlayerControl innerNetObject, Data data) {
            RolesManager.AssignRole(data.RoleInfo, data.Player);
        }


        public override Data Read(MessageReader reader) 
        {
            byte role = reader.ReadByte();
            var player = Utils.PlayerById(reader.ReadByte());
            return new Data(RolesManager.GetRoleInfoById(role), player );
        }

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.RoleInfo.id);
            writer.Write(data.Player.PlayerId);
        }
        

    }
}