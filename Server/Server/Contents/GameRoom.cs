using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Session;
using ServerCore;

namespace Server.Contents {
    public class GameRoom :IJobQueue{
        private const int max_capacity = 5;
        private Dictionary<int, ServerCore.Session> _sessions = new Dictionary<int, ServerCore.Session>();
        private JobQueue _jobQueue = new JobQueue();
        object l_sessions = new object();


        public int RoomID { get; set; }
        public int Capacity { get; set; } = max_capacity;
        public bool CanAccept { get { return Capacity > 0; } }

        public void EnterRoom(ClientSession session) {
            if(session == null)
                return;
            if(_sessions.ContainsKey(session.SessionID))
                return;

            lock(l_sessions) {
                _sessions.Add(session.SessionID, session);
            }

            S_Enter_Response enterPacket = new S_Enter_Response(){ SessionID = session.SessionID};
            session.Send(enterPacket);

            Console.WriteLine($"Client{session.SessionID} Entered Room{RoomID}");

            //TODO: Notice other players who's comin'
        }

        public void LeaveRoom(ClientSession session) {
            if(session == null)
                return;

            lock(l_sessions) {
                _sessions.Remove(session.SessionID);
            }

            //TODO: Broadcast Someone's Leave

            if(_sessions.Count == 0)
                DestroyRoom();
        }

        public void Broadcast(IMessage packet) {
            if(_sessions.Count == 0)
                return;

            lock(l_sessions) {
                foreach(ClientSession session in _sessions.Values) {
                    session.Send(packet);
                }
            }
        }

        public void DestroyRoom() {
            _sessions = null;
            _jobQueue = null;
            l_sessions = null;
            Console.WriteLine($"Try to Destory GameRoom {RoomID}");
            GameRoomManager.Instance.DestroyRoom(this);
        }

        public void Push(Action action) {
            _jobQueue.Push(action);
        }

        public void HandleMove(ClientSession session, IMessage move) {
            Broadcast(move);
        }
    }
}
