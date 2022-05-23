﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore {
    public class Connector {
        private Func<Session> _sessionFactory;

        public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory, int count = 10) {
            _sessionFactory = sessionFactory;

            for(int i = 0; i < count; i++) {
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);
                args.RemoteEndPoint = endPoint;
                args.UserToken = socket as object;

                RegisterConnect(args);
            }
        }

        private void RegisterConnect(SocketAsyncEventArgs args) {
            Socket socket = args.UserToken as Socket;
            bool pending = socket.ConnectAsync(args);

            if(pending == false)
                OnConnectCompleted(null, args);
        }

        private void OnConnectCompleted(object sender, SocketAsyncEventArgs args) {
            if(args.SocketError == SocketError.Success) {
                Console.WriteLine($"Connected to {args.ConnectSocket.RemoteEndPoint}");
                Session session = _sessionFactory.Invoke();
                session.Initialize(args.ConnectSocket);
                session.OnConnected();
            }
        }
    }
}
