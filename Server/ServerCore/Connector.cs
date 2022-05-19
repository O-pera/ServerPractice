using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore {
    public class Connector {
        private Socket _socket = null;
        private Action<Session> _onConnectedHandler;

        public void Connect(IPEndPoint endPoint, Action<Session> OnConnectedHandler) {
            _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);
            args.RemoteEndPoint = endPoint;
            _onConnectedHandler = OnConnectedHandler;

            RegisterConnect(args);
        }

        private void RegisterConnect(SocketAsyncEventArgs args) {
            bool pending = _socket.ConnectAsync(args);

            if(pending == false)
                OnConnectCompleted(null, args);
        }

        private void OnConnectCompleted(object sender, SocketAsyncEventArgs args) {
            if(args.SocketError == SocketError.Success) {
                Console.WriteLine($"Connected to {args.ConnectSocket.RemoteEndPoint}");
                Session session = new Session(args.ConnectSocket);
                session.Initialize();
                if(_onConnectedHandler != null)
                    _onConnectedHandler.Invoke(session);
            }
        }
    }
}
