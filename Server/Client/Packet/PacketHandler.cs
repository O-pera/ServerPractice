using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Packet {
    public class PacketHandler {
        public static void S_AcceptHandler(Session sin, IPacket pkt) {
            S_Accept packet = pkt as S_Accept;
            ServerSession session = sin as ServerSession;

            Console.WriteLine($@"UserID: [{session.UserID}] CodeName: [{packet.AuthCode}]");
        }
    }
}
