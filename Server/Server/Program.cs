using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server {
    public class Program {
        public static Listener _listener = new Listener();

        public static void Main(string[] args) {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, 1);

            while(true) {
                Thread.Sleep(1000);
                Console.WriteLine("Server Running....");
            }
        }
    }
}
