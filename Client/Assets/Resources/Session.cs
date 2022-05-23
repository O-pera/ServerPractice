using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore {
    public abstract class PacketSession : Session {
        protected static int Header_Size = 2;

        public sealed override int OnRecv(ArraySegment<byte> segment) {
            int processLen = 0;
            int processCount = 0;

            while(true) {
                if(segment.Count < Header_Size)
                    break;

                ushort size = BitConverter.ToUInt16(segment.Array, 0);
                if(segment.Count < size)
                    break;

                processLen += size;
                ArraySegment<byte> pSegment = new ArraySegment<byte>(segment.Array, 0, size);
                OnRecvPacket(pSegment); processCount++;

                segment = new ArraySegment<byte>(segment.Array, segment.Offset + size, segment.Count - size);
            }

            return processLen;
        }

        public abstract void OnRecvPacket(ArraySegment<byte> segment);
    }

    public abstract class Session {

        protected Socket _socket;
        public string Socket { 
            get {
                if(Disconnected)
                    return "Nothing's Connected";
                else
                    return _socket.RemoteEndPoint.ToString();
            } 
        }
        protected int _disconnected = -1;
        protected bool Disconnected {
            get { return _disconnected == 1; }
        }

        private SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        private object _lock = new object();

        private SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        private RecvBuffer _recvBuffer = new RecvBuffer(65535);

        public virtual void Initialize(Socket socket) {
            if(Interlocked.Exchange(ref _disconnected, 0) == 0)
                return;

            _socket = socket;

            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

            RegisterRecv();
        }

        public void Disconnect() {
            try {
                Console.WriteLine($"Disconnecting with {_socket.RemoteEndPoint}");
                if(Interlocked.Exchange(ref _disconnected, 1) == 1)
                    return;

                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
            catch(Exception e) {
                Console.WriteLine($"Error Ocurred: {e.Message}");
            }
        }

        #region Send Functions
        public void Send(ArraySegment<byte> packet) {
            if(Disconnected || _socket == null)
                return;

            RegisterSend(packet);
        }
        private void RegisterSend(ArraySegment<byte> buffer) {
            try {
                lock(_lock) {
                    _sendArgs.SetBuffer(buffer.Array, buffer.Offset, buffer.Count);
                    bool pending = _socket.SendAsync(_sendArgs);

                    if(pending == false)
                        OnSendCompleted(null, _sendArgs);
                }
            } catch(Exception e) {
                Console.WriteLine($"RegisterSend Failed: {e.ToString()}");
            }
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs args) {

        }

        #endregion

        #region Recv Functions
        private void RegisterRecv() {
            if(Disconnected || _socket == null)
                return;

            _recvBuffer.Clear();
            ArraySegment<byte> segment = _recvBuffer.WriteSegment;
            _recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

            bool pending = _socket.ReceiveAsync(_recvArgs);

            if(pending == false) {
                OnRecvCompleted(null, _recvArgs);
            }
        }

        private void OnRecvCompleted(object sender, SocketAsyncEventArgs args) {
            if(args.BytesTransferred > 0 && args.SocketError == SocketError.Success) {
                try {
                    if(_recvBuffer.OnWrite(args.BytesTransferred) == false) {
                        Disconnect();
                        return;
                    }

                    int processLen = OnRecv(_recvBuffer.DataSegment);

                    if(args.BytesTransferred != processLen) {
                        Disconnect();
                        return;
                    }

                    if(_recvBuffer.OnRead(args.BytesTransferred) == false) {
                        Disconnect();
                        return;
                    }

                    RegisterRecv();
                }
                catch(Exception e) {
                    Console.WriteLine($"OnRecvCompleted Failed: {e.ToString()}");
                }
            }
        }
        #endregion

        #region Abstract Functions

        public abstract void OnConnected();
        public abstract void OnSend();
        public abstract int OnRecv(ArraySegment<byte> segment);
        public abstract void OnDisconnected();

        #endregion
    }
}
