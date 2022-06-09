using Client.Session;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager{
    private Connector _connector = new Connector();
    private ServerSession _session = new ServerSession();
    public void Start(){
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        _connector.Connect(endPoint, () => { return _session; }, 1);
    }

    public void Update(){
        List<PacketData> list = PacketQueue.Instance.PopAll();
        if(list == null)
            return;

        foreach(PacketData packet in list) {
            PacketManager.Instance.GetPacketHandler(packet.ID).Invoke(_session, packet.Packet);
        }
    }

    public void Send(IMessage packet) {
        _session.Send(packet);
    }

    public void OnApplicationQuit() {
        _session.Disconnect();
    }
}
