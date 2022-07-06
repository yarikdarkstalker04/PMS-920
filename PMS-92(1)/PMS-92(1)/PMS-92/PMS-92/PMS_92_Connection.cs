using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PMS_92
{
    class PMS_92_Connection : IConnection
    {
        public IPAddress IP { get; set; }
        public int TCP { get; set; }

        public PMS_92_Connection(IPAddress ip, int tcp)
        {
            IP = ip;
            TCP = tcp;
        }
        public PMS_92_Connection()
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            IP = addr[0];
            TCP = 502;
        }
        void Create_TCP_Listener()
        {
            TcpListener tcpListener = new TcpListener(IP, TCP);
            tcpListener.Start();
        }
    }
}
