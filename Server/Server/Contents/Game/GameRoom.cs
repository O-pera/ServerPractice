using Server.Session;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contents.Game {
    public class GameRoom : IJobQueue{
        public GameRoom(int capacity = -1) {
            _jobQueue = new JobQueue();
            _sessions = new Dictionary<int, ClientSession>();
            l_sessions = new object();

            if(capacity != -1)
                max_capacity = capacity;

            this.capacity = max_capacity - _sessions.Count;
        }
        ~GameRoom() => Destroy();

        private readonly int max_capacity = 50;


        JobQueue _jobQueue;
        Dictionary<int, ClientSession> _sessions;
        object l_sessions;
        private int capacity;
        public GameRoom CanAccept { 
            get {
                if(Interlocked.Decrement(ref capacity) <= 0) {
                    Interlocked.Increment(ref capacity);
                    return null;
                }

                return this;
            } 
        }
        public int RoomID { get; set; }

        #region Queue Management Systems
        public void Push(Action action) {
            _jobQueue.Push(action);
        }

        public void Pop() {
            List<Action> list = _jobQueue.PopAll();

            if(list.Count > 0) {
                foreach(Action action in list) {
                    action.Invoke();
                }
            }
        }

        public void Flush() {
            List<Action> list = _jobQueue.PopAll();
            foreach(Action action in list) {
                action.Invoke();
            }
        }

        #endregion

        #region Session Management System

        public void EnterRoom(ClientSession session) {
            lock(l_sessions) {
                _sessions.Add(session.SessionID, session);

                Console.WriteLine($"Session({session.SessionID}) Entered Room {RoomID}");
                //TODO: Send Enter Packet to all of sessions
            }
        }

        public void LeaveRoom(ClientSession session) {
            lock(l_sessions) {
                if(session.IsPlayer) {
                    //TODO: Saving Player's Data
                }

                _sessions.Remove(session.SessionID);
                Console.WriteLine($"Session({session.SessionID}) Leaved Room");
                //TODO: Send Packet to all of sessions

            }
        }

        /// <summary>
        /// 해당 세션에 직접 접근할 때 필요할 수 있음.
        /// 함수 재정의 요망
        /// </summary>
        /// <param name="sessionID">세션ID 입력</param>
        /// <returns>return ClientSession if session added to _sessions, or return false </returns>
        public ClientSession PeekSession(int sessionID) {
            ClientSession session = null;
            _sessions.TryGetValue(sessionID, out session);

            return session;
        }

        #endregion

        #region Room Management System

        public bool Start(int roomID) {
            RoomID = roomID;

            //TODO: Instantiate room's Instances

            return true;
        }

        public void Destroy() {
            //TODO: Manage Instance's resources
            Interlocked.Exchange(ref capacity, 0);

            foreach(ClientSession session in _sessions.Values) {
                session.Disconnect();
            }
        }

        #endregion

        #region Room Contents System

        public void CharacterMove(ClientSession session, ArraySegment<byte> packet) {
            if(session.IsDisconnected) {
                Push(() => { LeaveRoom(session); });
                return;
            }

            Console.WriteLine($"Session({session.SessionID}) moved");
            //TODO: 1. Make Player Character's movement


            //TODO: 2. Send Packet to all of sessions


        }

        //TODO: Add Room Contents After Architect Packet Protocol

        #endregion
    }
}
