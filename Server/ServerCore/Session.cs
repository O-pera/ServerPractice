using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore {
    public abstract class PacketSession : Session {
        private readonly static int Header_Size = 2;
        public sealed override int OnRecv(ArraySegment<byte> segment) {
            int processLen = 0;


            processLen = segment.Count;
            OnRecvPacket(segment);
            //while(true) {
            //    if(segment.Count < Header_Size)
            //        break;

            //    ushort dataSize = BitConverter.ToUInt16(segment.Array, segment.Offset);
            //    if(segment.Count < dataSize)
            //        break;

            //    OnRecvPacket(new ArraySegment<byte>(segment.Array, segment.Offset, dataSize));

            //    processLen += dataSize;
            //    segment = new ArraySegment<byte>(segment.Array, segment.Offset + dataSize, segment.Count - dataSize);
            //}

            return processLen;
        }
        public abstract void OnRecvPacket(ArraySegment<byte> segment);
    }
    public abstract class Session {
        private Socket _socket;
        private int _verified = 1;
        private int _disconnected = 1;

        private SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        private RecvBuffer _recvBuffer;

        private SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        private object l_send = new object();
        private Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        private object l_sendQueue = new object();
        private List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public int SessionID { get; set; }
        public bool IsDisconnected { 
            get {
                return _disconnected == 1; 
            } 
        }
        public bool IsVerified {
            get {
                return _verified == 1;
            }
        }

        public void Start(Socket socket, int recvBufferSize = 65535) {
            _socket = socket;
            OnConnected(_socket.RemoteEndPoint);
            _recvBuffer = new RecvBuffer(recvBufferSize);

            _recvArgs.SetBuffer(_recvBuffer.FreeSegment);
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            Interlocked.Exchange(ref _disconnected, 0);
            RegisterRecv();
        }

        public void Disconnect() {
            if(Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;
            OnDisconnected(_socket.RemoteEndPoint);
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void Send(ArraySegment<byte> buffer) {
            if(_disconnected == 1)
                return;

            lock(l_sendQueue) {
                _sendQueue.Enqueue(buffer);
            }

            lock(l_send) {
                if(_pendingList.Count == 0)
                    RegisterSend();
            }
        }

        public void Send(List<ArraySegment<byte>> bufferList) {

        }

        private void RegisterSend() {
            try {
                while(_sendQueue.Count > 0) {
                    _pendingList.Add(_sendQueue.Dequeue());
                }
                _sendArgs.BufferList = _pendingList;
                bool pending = _socket.SendAsync(_sendArgs);

                if(pending == false)
                    OnSendCompleted(null, _sendArgs);
            } catch(Exception e) {
                Console.WriteLine($"RegisterSend Failed! {e}");
            }
        }

        private void OnSendCompleted(object? sender, SocketAsyncEventArgs args) {
            lock(l_send) {
                if(args.SocketError == SocketError.Success) {
                    if(_sendQueue.Count > 0) {
                        RegisterSend();
                    }
                    else {
                        _pendingList.Clear();
                    }
                }
                else {
                    Disconnect();
                }
            }
        }

        private void RegisterRecv() {
            if(_disconnected == 1)
                return;

            try {
                _recvBuffer.Clear();
                ArraySegment<byte> segment = _recvBuffer.FreeSegment;
                _recvArgs.SetBuffer(segment);
                bool pending = _socket.ReceiveAsync(_recvArgs);

                if(pending == false)
                    OnRecvCompleted(null, _recvArgs);
            } catch(Exception e) {
                Console.WriteLine($"RegisterRecv Failed! {e}");
            }
        }

        private void OnRecvCompleted(object? sender, SocketAsyncEventArgs args) {
            if(args.SocketError == SocketError.Success && args.BytesTransferred > 0) {
                if(_recvBuffer.OnWrite(args.BytesTransferred) == false) {
                    Disconnect();
                    return;
                }

                int processLen = OnRecv(_recvBuffer.DataSegment);
                if(processLen < 0 || _recvBuffer.DataSize < processLen) {
                    Disconnect();
                    return;
                }

                if(_recvBuffer.OnRead(processLen) == false) {
                    Disconnect();
                    return;
                }
            }
            else {
                Disconnect();
            }

            RegisterRecv();
        }

        #region Abstract Functions

        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnDisconnected(EndPoint endPoint);
        public abstract void OnSend(int numOfBytes);
        public abstract int OnRecv(ArraySegment<byte> segment);
    }

    #endregion
}