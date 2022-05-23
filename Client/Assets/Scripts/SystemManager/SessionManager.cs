using Network;
using Network.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SessionManager : ManagerClass{
    private ServerSession _session = null;

    public override void Initialize() {
        Type = ManagerType.Static;
        PacketManager.Instance.Initialize()
    }

    public ServerSession CreateSession() {
        _session = new ServerSession();
        return _session;
    }

    public void Send(IPacket packet) {
        _session.Send(packet.Write());
    }
}

