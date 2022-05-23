﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore {
    public  class Listener {
        private Socket _listenSocket;
        private Func<Session> _sessionFactory;
        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory, int backlog = 10) {
            _sessionFactory = sessionFactory;

            _listenSocket = new Socket(endPoint.AddressFamily, 
                                       SocketType.Stream,
                                       ProtocolType.Tcp);

            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(backlog);

            for(int i = 0; i < backlog; i++) {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);

                RegisterAccept(args);
            }
        }

        private void RegisterAccept(SocketAsyncEventArgs args) {
            bool pending = _listenSocket.AcceptAsync(args);

            if(pending == false) {
                OnAcceptCompleted(null, args);
            }
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs args) {
            if(args.SocketError == SocketError.Success) {
                //TODO: 세션 생성 및 연결
                Console.WriteLine($"Connected to: {args.AcceptSocket.RemoteEndPoint}");
                Session session = _sessionFactory.Invoke();
                session.Initialize(args.AcceptSocket);
                session.OnConnected();
            }

            args.AcceptSocket = null;
            RegisterAccept(args);
        }
    }
}