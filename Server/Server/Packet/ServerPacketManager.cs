using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packet {
    public class PacketManager {
        #region Singleton
        public static PacketManager Instance { get; } = new PacketManager();

        #endregion
        private Dictionary<ushort, Action<Session, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<Session, ArraySegment<byte>>>();
        private Dictionary<ushort, Action<Session, IPacket>> _handler = new Dictionary<ushort, Action<Session, IPacket>>();
        private Action<Session, IPacket>? _handleCallback = null;

        public void Initialize(Action<Session, IPacket>? handleCallback = null) {
            if(handleCallback != null) {
                _handleCallback -= handleCallback;
                _handleCallback += handleCallback;
            }

            //TODO: RPC 만들기
            _onRecv.Add(( ushort )PacketType.C_TryConnect, MakePacket<C_TryConnect>);
            _handler.Add(( ushort )PacketType.C_TryConnect, PacketHandler.C_TryConnectHandler);
        }

        public void OnRecvPacket(Session session, ArraySegment<byte> packet) {
            ushort type = BitConverter.ToUInt16(packet.Array, 2);

            Action<Session, ArraySegment<byte>> action = null;
            if(_onRecv.TryGetValue(type, out action) == true) {
                action.Invoke(session, packet);
            }
        }

        private void MakePacket<T>(Session session, ArraySegment<byte> segment) where T : IPacket, new() {
            T packet = new T();
            packet.Read(segment);

            Action<Session, IPacket> action = null;
            if(_handleCallback != null) {
                _handleCallback.Invoke(session, packet);
                return;
            }
            if(_handler.TryGetValue(( ushort )packet.Type, out action) != null) {
                action.Invoke(session, packet);
                return;
            }
        }
    }
}
