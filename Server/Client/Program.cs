using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client {
    public class Program {
        private static Connector _connector = new Connector();

        public static void OnConnectedHandler(Session session) {
            while(true) {
                session.Send();
                Console.WriteLine("Sended to Server");
                Thread.Sleep(1000);
            }
        }

        public static void Main(string[] args) {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _connector.Connect(endPoint, OnConnectedHandler);

            while(true) {
                Thread.Sleep(1000);
            }
        }
    }
}
