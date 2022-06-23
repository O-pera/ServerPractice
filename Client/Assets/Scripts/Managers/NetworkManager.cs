using Client.Session;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : Manager, IManagerStart, IManagerUpdate, IManagerOnApplicationQuit{
    
    private Connector _connector = new Connector();
    private ServerSession _session = new ServerSession();

    public void Start() {
        PacketManager.Instance.CustomHandler = (id, packet) => { PacketQueue.Instance.Push(id, packet); };
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        _connector.Connect(endPoint, () => { return _session; }, 1);
    }

    public void Update() {
        HandlePacket();

        if(Input.GetKeyDown(KeyCode.Space)) {
            C_Enter enter = new C_Enter();
            Send(enter);
        }
    }
    public void OnApplicationQuit() {
        _session.Disconnect();
    }

    public void Send(IMessage packet) {
        _session.Send(packet);
    }

    private void HandlePacket() {
        List<PacketData> list = PacketQueue.Instance.PopAll();
        if(list == null)
            return;

        foreach(PacketData packet in list) {
            PacketManager.Instance.GetPacketHandler(packet.ID).Invoke(_session, packet.Packet);
        }
    }
}
