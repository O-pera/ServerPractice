using Server.Packet;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server {
    public class ClientSession : PacketSession {
        public override void OnConnected() {
            
        }

        public override void OnDisconnected() {
            
        }

        public override void OnRecvPacket(ArraySegment<byte> segment) {
            PacketManager.Instance.OnRecvPacket(this, segment);
        }

        public override void OnSend() {
            
        }
    }
}
