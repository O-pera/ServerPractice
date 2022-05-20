using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packet {
    public class PacketHandler {
        public static void C_TryConnectHandler(Session sin, IPacket pkt) {
            C_TryConnect packet = pkt as C_TryConnect;
            ClientSession session = sin as ClientSession;

            S_Accept acceptPacket = new S_Accept();
            acceptPacket.AuthCode = Random.Shared.Next(101, 200);
            session.Send(acceptPacket.Write());

            Console.WriteLine($@"From: [{session.Socket}] UserID: [{packet.UserID}] CodeName: [{acceptPacket.AuthCode}]");
        }
    }
}
