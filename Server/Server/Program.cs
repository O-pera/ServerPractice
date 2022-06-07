using Google.Protobuf.Protocol;
using Server.Contents;
using Server.Session;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server {
    public class Program {
        private static Listener _listener = new Listener();

        public static void Main(string[] args) {
            InitializeServer();
        }

        private static void InitializeServer() {
            string host = Dns.GetHostName();    //www.naver.com
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Start(endPoint, SessionManager.Instance.Generate, count: 10);
            
            
            while(true) {

            }
        }


    }
}
