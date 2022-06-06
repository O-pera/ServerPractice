using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore {
    public class Listener {
        Socket _socket;
        Func<Session> _sessionFactory;

        public void Start(IPEndPoint endPoint, Func<Session> sessionFactory, int backlog = 10, int count = 1) {
            _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory = sessionFactory;

            _socket.Bind(endPoint);
            _socket.Listen(backlog);

            for(int i = 0; i < count; i++) {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);

                RegisterAccept(args);
            }
        }

        private void RegisterAccept(SocketAsyncEventArgs args) {
            try {
                bool pending = _socket.AcceptAsync(args);

                if(pending == false)
                    OnAcceptCompleted(null, args);
            }catch(Exception e) {
                Console.WriteLine($"RegisterAccept Failed! {e}");
            }
        }

        private void OnAcceptCompleted(object? sender, SocketAsyncEventArgs args) {
            if(args.SocketError == SocketError.Success) {
                try {
                    Session session = _sessionFactory.Invoke();
                    session.Start(args.AcceptSocket);

                } catch(Exception e) {
                    Console.WriteLine($"OnAcceptCompleted Failed! {e}");
                }
            }

            args.AcceptSocket = null;
            RegisterAccept(args);
        }
    }
}
