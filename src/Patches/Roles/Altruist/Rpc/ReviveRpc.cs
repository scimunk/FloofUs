using System;
using Reactor.Networking;
using Reactor;
using FloofUs;
using Hazel;

namespace FloofUs
{
    [RegisterCustomRpc((uint) CustomRpcCalls.AltruistRevive)]
    public class ReviveRpc : PlayerCustomRpc<FloofUsPlugin, ReviveRpc.Data>
    {

        public ReviveRpc(FloofUsPlugin plugin, uint id) : base(plugin, id){
            
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;

        public readonly struct Data
        {   
            public readonly PlayerControl AltruistPlayer;
            public readonly PlayerControl Player;

            public Data(PlayerControl altruistPlayer, PlayerControl player)
            {
                AltruistPlayer = altruistPlayer;
                Player = player;
            }
        }

        public override void Handle(PlayerControl innerNetObject, Data data) {
            //Roles.Role.GetRole<Roles.Altruist>(altruistPlayer);
        }


        public override Data Read(MessageReader reader) 
        {

            byte altruistPlayerId = reader.ReadByte();
            byte playerId = reader.ReadByte();
            var altruistPlayer = Utils.PlayerById(altruistPlayerId);
            var player = Utils.PlayerById(playerId);
            return new Data(altruistPlayer, player );
        }

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.AltruistPlayer.PlayerId);
            writer.Write(data.Player.PlayerId);
        }
        

    }
}