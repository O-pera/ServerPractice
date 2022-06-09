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

        public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory, int count = 1) {
            _sessionFactory = sessionFactory;

            for(int i = 0; i < count; i++) {
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.RemoteEndPoint = endPoint;
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompletd);
                args.UserToken = socket;

                RegisterConnect(args);
            }
        }

        private void RegisterConnect(SocketAsyncEventArgs args) {
            Socket socket = args.UserToken as Socket;

            bool pending = socket.ConnectAsync(args);
            if(pending == false)
                OnConnectCompletd(null, args);
        }

        private void OnConnectCompletd(object? sender, SocketAsyncEventArgs args) {
            if(args.SocketError == SocketError.Success) {
                Session session = _sessionFactory.Invoke();
                session.Start(args.ConnectSocket);
                session.OnConnect(args.ConnectSocket.RemoteEndPoint);
            }
        }
    }
}
