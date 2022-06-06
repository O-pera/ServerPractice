using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client.Session {
    public class ServerSession : PacketSession {
        public int SessionID { get; set; }
        public override void OnConnected(EndPoint endPoint) {
            Console.WriteLine($"Connected to: {endPoint}");
        }
        public override void OnDisconnected(EndPoint endPoint) {

        }

        public override void OnRecvPacket(ArraySegment<byte> segment) {
            string message = Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
            Console.WriteLine($"From Remote: {message}");
        }

        public override void OnSend(int numOfBytes) {

        }
    }
}
