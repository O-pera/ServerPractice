using Network;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : ManagerClass{
    Connector _connector = new Connector();
    Session _session;

    public override void Initialize() {
        Type = ManagerType.Updatable;

        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        _connector.Connect(endPoint, Managers.Instance.Session.CreateSession);
    }

    public override void Update() {
        
    }


}
