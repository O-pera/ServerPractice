using Server.Contents.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session {
    public class ClientSession : PacketSession {
        public Boolean IsPlayer { get; set; } = false;
        public GameRoom Room { get; set; }

        public override void OnConnected(EndPoint endPoint) {
            Console.WriteLine($"Connected to: {endPoint}");
            GameRoomManager.Instance.TryEnterRoom(this);
        }

        public override void OnDisconnected(EndPoint endPoint) {

        }

        public override void OnRecvPacket(ArraySegment<byte> segment) {
            string message = Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
            Console.WriteLine($"From Remote: {message}, I'm {SessionID} in {Room.RoomID}");
        }

        public override void OnSend(int numOfBytes) {

        }
    }
}
