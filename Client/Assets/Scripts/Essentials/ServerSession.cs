using Network.Packet;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network {
    public class ServerSession : PacketSession {
        public int UserID { get; set; }
        public override void OnConnected() {
            C_TryConnect tryPacket = new C_TryConnect();
            tryPacket.UserID = UnityEngine.Random.Range(0, 100);
            UserID = tryPacket.UserID;
            Send(tryPacket.Write());
            Console.WriteLine($"Sended to Server. My UserID is {UserID}");
        }

        public override void OnDisconnected() {
            Disconnect();
        }

        public override void OnRecvPacket(ArraySegment<byte> segment) {
            PacketManager.Instance.OnRecvPacket(this, segment);
        }

        public override void OnSend() {
            
        }
    }
}
