using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contents {
    public class GameRoomManager {
        #region Singleton
        private static GameRoomManager _instance;
        public static GameRoomManager Instance { get { return _instance; } }
        static GameRoomManager() {
            _instance = new GameRoomManager();
        }
        #endregion

        private Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
        private object l_rooms = new object();
        private int _roomID = 0;
        public GameRoom Generate() {
            GameRoom room = null;
            lock(l_rooms) {
                room = new GameRoom(){RoomID = _roomID++};
                _rooms.Add(room.RoomID, room);
            }

            return room;
        }

        public void DestroyRoom(GameRoom room) {
            if(room == null)
                return;
            bool success = false;

            lock(l_rooms) {
                success = _rooms.Remove(room.RoomID);
            }
            Console.WriteLine($"GameRoom{room.RoomID} Removing Result: {success}");
        }

        public void TryEnterRoom(ClientSession session) {
            if(session == null)
                return;

            GameRoom tryable = null;
            lock(l_rooms) {
                foreach(GameRoom room in _rooms.Values) {
                    if(room.CanAccept) {
                        tryable = room;
                        break;
                    }
                }

                if(tryable == null)
                    tryable = Generate();

                tryable.Capacity--;
            }

            tryable.Push(() => { tryable.EnterRoom(session); });
            session.Room = tryable;
        }
    }
}
