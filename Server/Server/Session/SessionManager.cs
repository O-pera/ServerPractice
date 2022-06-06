using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session {
    public class SessionManager {
        public static SessionManager Instance { get; } = new SessionManager();
        private Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        private object l_sessions = new object();
        private int _sessionID = 0;

        public ClientSession Generate() {
            lock(l_sessions) {
                ClientSession session = new ClientSession();
                session.SessionID = _sessionID++;
                _sessions.Add(session.SessionID, session);
                return session;
            }
        }

        public bool RemoveSession(ClientSession session) {
            try {
                lock(l_sessions) {
                    bool success = _sessions.Remove(session.SessionID);

                    return success;
                }
            }catch(Exception e) {
                Console.WriteLine($"RemoveSession Failed: {e}");
                return false;
            }
        }
    }
}
