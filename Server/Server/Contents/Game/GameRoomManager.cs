using Server.Session;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contents.Game {
    public class GameRoomManager {
        #region Singleton
        public static GameRoomManager Instance { get; private set; } = new GameRoomManager();
        #endregion

        private GameRoomManager() {
            _rooms = new Dictionary<int, GameRoom>();
            _roomID = 0;
            l_rooms = new object();
        }
        private Dictionary<int, GameRoom> _rooms;
        private object l_rooms;

        private static int _roomID;

        #region GameRoomManager Functions

        public GameRoom Generate() {
            GameRoom room = new GameRoom();
            int roomID = Interlocked.Exchange(ref _roomID, _roomID + 1);
            room.Start(roomID);

            lock(l_rooms) {
                _rooms.Add(room.RoomID, room);
            }

            return room;
        }

        public void RemoveRoom(GameRoom room) {
            //TODO: Manage room's instances
            room.Push(() => { room.Destroy(); });

            lock(l_rooms) {
                _rooms.Remove(room.RoomID);
            }
        }

        public void TryEnterRoom(ClientSession session) {
            if(session.IsVerified == false) {
                session.Disconnect();
                SessionManager.Instance.RemoveSession(session);
                return;
            }

            GameRoom room = null;
            lock(l_rooms) {
                foreach(GameRoom r in _rooms.Values) {
                    room = r.CanAccept;
                }

                if(room == null)
                    room = Generate();
            }

            session.Room = room;
            room.Push(() => { room.EnterRoom(session); });

            Console.WriteLine($"Entered Room {room.RoomID}");
        }
        #endregion
    }
}
