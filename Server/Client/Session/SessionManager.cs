using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Session {
    public class SessionManager {
        public static SessionManager Instance { get; } = new SessionManager();
        private Dictionary<int, ServerSession> _sessions = new Dictionary<int, ServerSession>();
        private object _lock = new object();
        private int _sessionID = 0;

        public ServerSession Generate() {
            lock(_lock) {
                ServerSession session = new ServerSession();
                session.SessionID = _sessionID++;
                _sessions.Add(session.SessionID, session);
                return session;
            }
        }

        public bool RemoveSession(ServerSession session) {
            ServerSession s = null;

            bool result = _sessions.TryGetValue(session.SessionID, out s);
            if(result == true) {
                lock(_lock) {
                    _sessions.Remove(session.SessionID);
                }
            }

            return result;
        }

        public void SendForEach() {
            lock(_lock) {
                foreach(ServerSession session in _sessions.Values) {
                    string message = "Hello Server!";
                    ArraySegment<byte> segment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message), 0, message.Length);
                    session.Send(segment);
                }
            }
        }
    }
}
