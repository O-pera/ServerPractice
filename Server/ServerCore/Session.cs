using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore {
    public class Session {
        protected Socket _socket;
        protected int _disconnected = 1;


        private SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        private object _lock = new object();

        private SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        private RecvBuffer _recvBuffer = new RecvBuffer(65535);


        public Session(Socket socket) {
            _socket = socket;
        }

        public void Initialize() {
            if(Interlocked.Exchange(ref _disconnected, 0) == 0)
                return;

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
        public void Send() {
            if(_disconnected == 1 || _socket == null)
                return;

            string data = "FUCKING HELL";
            byte[] buffer = Encoding.UTF8.GetBytes(data);

            RegisterSend(new ArraySegment<byte>(buffer, 0, buffer.Length));
        }
        private void RegisterSend(ArraySegment<byte> buffer) {
            lock(_lock) {
                _sendArgs.SetBuffer(buffer);
                bool pending = _socket.SendAsync(_sendArgs);

                if(pending == false)
                    OnSendCompleted(null, _sendArgs);
            }
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs args) {

        }
        #endregion

        #region Recv Functions
        private void RegisterRecv() {
            if(_disconnected == 1 || _socket == null)
                return;

            _recvBuffer.Clear();
            _recvArgs.SetBuffer(_recvBuffer.WriteSegment);

            bool pending = _socket.ReceiveAsync(_recvArgs);

            if(pending == false) {
                OnRecvCompleted(null, _recvArgs);
            }
        }

        private void OnRecvCompleted(object sender, SocketAsyncEventArgs args) {
            if(args.BytesTransferred > 0 && args.SocketError == SocketError.Success) {
                if(_recvBuffer.OnWrite(args.BytesTransferred) == false)
                    return;

                Console.WriteLine($"From:[{_socket.RemoteEndPoint}] Data: [{Encoding.UTF8.GetString(_recvBuffer.DataSegment)}]");

                if(_recvBuffer.OnRead(args.BytesTransferred) == false)
                    return;


                RegisterRecv();
            }
        }
        #endregion
    }
}
